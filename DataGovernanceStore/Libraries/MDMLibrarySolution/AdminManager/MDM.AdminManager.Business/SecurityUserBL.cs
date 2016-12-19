using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Transactions;

namespace MDM.AdminManager.Business
{
    using BusinessObjects;
    using BusinessObjects.DataModel;
    using BusinessObjects.Diagnostics;
    using CacheManager.Business;
    using ConfigurationManager.Business;
    using Core;
    using Core.Exceptions;
    using Data;
    using DataModelManager.Business;
    using Interfaces;
    using Core.DataModel;
    using MessageManager.Business;
    using Utility;

    /// <summary>
    /// Specifies operations for security user
    /// </summary>
    public class SecurityUserBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting reference of user Preferences.
        /// </summary>
        private IDataModelManager _iUserPreferencesManager = null;

        /// <summary>
        /// Filed denoting DataLocale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        #endregion Fields

        #region Constructors

        public SecurityUserBL()
        {
            GetSecurityPrincipal();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }
         /// <summary>
        /// Initializes a new instance of the securityUser BL.
        /// </summary>
        public SecurityUserBL(IOrganizationManager iOrganizationManager, IContainerManager iContainerManager, IHierarchyManager iHierarchyManager)
        {
            GetSecurityPrincipal();
            _iUserPreferencesManager = new UserPreferencesBL(iOrganizationManager, iContainerManager, iHierarchyManager);
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="countFrom"></param>
        /// <param name="countTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public Collection<SecurityUser> GetAllUsers(Int32 userType, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, String userLogin)
        {
            return new SecurityUserDA().GetAllUsers(userType, countFrom, countTo, sortColumn, searchColumn, searchParameter, userLogin);
        }
        
        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <param name="callerContext">Context of application making call to this API</param>
        /// <returns>Collection of users</returns>
        public SecurityUserCollection GetAll(CallerContext callerContext)
        {
            #region Parameter Validation
            
            if (callerContext == null)
            {
                String errorMessage = "CallerContext cannot be null";
                throw new MDMOperationException("111846", errorMessage, "SecurityUserBL", String.Empty, "GetAll");
            }

            #endregion Parameter Validation

            return GetUsers(callerContext);
        }

        /// <summary>
        /// Get user for given user id
        /// </summary>
        /// <param name="userId">Id of user to fetch data for</param>
        /// <param name="callerContext">Context of application making call to this API</param>
        /// <returns>User having given UserId</returns>
        public SecurityUser GetById(Int32 userId, CallerContext callerContext)
        {
            #region Parameter Validation

            if (callerContext == null)
            {
                throw new MDMOperationException("111823", String.Empty, "SecurityUserBL", String.Empty, "GetByUserId");//CallerContext is null or empty
            }

            if (userId < 1)
            {
                throw new MDMOperationException("112082", String.Empty, "SecurityUserBL", String.Empty, "GetByUserId");//UserId cannot be less than 1
            }


            #endregion Parameter Validation

            SecurityUser user = null;
            SecurityUserCollection users = GetUsers(callerContext, 0, userId);

            if (users != null && users.Count > 0)
            {
                user = users.FirstOrDefault();
            }

            return user;
        }

		/// <summary>
        /// Get Security User details.
        /// </summary>
        /// <param name="userLogin">Indicates the user LoginId</param>
        /// <param name="callerContext">Caller context to indicate application and modlue which has called an API</param>
        /// <param name="includeDisabledUser">Indicates whether disabled user should be ignored or not</param>
        /// <returns></returns>
        public SecurityUser GetUser(String userLogin, CallerContext callerContext = null, Boolean includeDisabledUser = false)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("SecurityUserBL.GetUser", MDMTraceSource.DataModel, false);
            }

            #region Parameter Validation

            if (String.IsNullOrEmpty(userLogin))
            {
                String errorMessage = "User login can not be null or empty.";
                throw new MDMOperationException("112689", errorMessage, "AdminManager.SecurityUserBL", String.Empty, "GetUser");
            }

            #endregion

            SecurityUser securityUser = new SecurityUser();

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "User login : " + userLogin, MDMTraceSource.DataModel);
                    if (callerContext != null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);
                }
                }

                Collection<String> userLoginlist = new Collection<String> { userLogin };
                SecurityUserCollection securityUsers = GetUsers(userLoginlist, null, includeDisabledUser);

                if (securityUsers != null && securityUsers.Count > 0)
                {
                    securityUser = securityUsers.FirstOrDefault();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("SecurityUserBL.GetUser", MDMTraceSource.DataModel);
            }
            }

            return securityUser;
        }

        /// <summary>
        /// Get SecurityUserCollection based on the user logins
        /// </summary>
        /// <param name="userLogins">Indicates the list of MDM User Logins</param>
        /// <returns></returns>
        public SecurityUserCollection GetUsersByLogins(Collection<String> userLogins)
        {
            //TODO:: Validate userLogins
            return GetUsers(userLogins, null);
        }

        /// <summary>
        /// Get SecurityUserCollection based on the user Ids.
        /// </summary>
        /// <param name="userIds">Indicates the list of MDM User Ids</param>
        /// <returns></returns>
        public SecurityUserCollection GetUsersByIds(Collection<Int32> userIds)
        {
            //TODO:: Validate userIds
            return GetUsers(null, userIds);
        }

        /// <summary>
        /// Get available Users based on the Role Id.
        /// </summary>
        /// <param name="roleId">Indicates the Role Id</param>
        /// <returns>return a Collection of SecurityUser </returns>
        public Collection<SecurityUser> GetUsersInRole(Int32 roleId)
        {
            SecurityUserDA securityUserDA = new SecurityUserDA();
            Collection<SecurityUser> usersCollection = null;
            String userName = String.Empty;

            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            SecurityUserCollection users = securityUserDA.Get(userName, roleId);

            LoadSecurityRoles(users);

            if (users != null && users.Count > 0)
            {
                usersCollection = new Collection<SecurityUser>();

                foreach (SecurityUser user in users)
                {
                    usersCollection.Add(user);
                }
            }

            return usersCollection;
        }

        /// <summary>
        /// Get Security User details.
        /// </summary>
        /// <param name="userLogins">Indicates collection of user LoginId</param>
        /// <param name="callerContext">Caller context to indicate application and modlue which has called an API</param>
        /// <param name="includeDisabledUsers">Indicates whether disabled users should be ignored or not</param>
        /// <returns>Returns security user collection</returns>
        public SecurityUserCollection GetUsersByLoginNames(Collection<String> userLogins, CallerContext callerContext = null, Boolean includeDisabledUsers = false)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("SecurityUserBL.GetUsersByLogin", MDMTraceSource.DataModel, false);
            }

            SecurityUserCollection securityUsers = new SecurityUserCollection();

            try
            {
                if (Constants.TRACING_ENABLED && callerContext != null)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);
                }

                securityUsers = GetUsers(userLogins, null, includeDisabledUsers);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("SecurityUserBL.GetUsersByLogin", MDMTraceSource.DataModel);
            }
            }

            return securityUsers;
        }

        /// <summary>
        /// Gets collection of users under a specifies role
        /// </summary>
        /// <param name="securityRoleShortName"> Short name of the role.</param>
        /// <param name="callerContext">Caller context to denote application and module which has called an API</param>
        /// <returns>returns collection of users</returns>
        public SecurityUserCollection GetUsersInRole(String securityRoleShortName, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("SecurityUserBL.GetUsersInRole", MDMTraceSource.DataModel, false);
            }

            SecurityUserCollection securityUsers = new SecurityUserCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Role short name- " + securityRoleShortName, MDMTraceSource.DataModel);

                    if (callerContext != null)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);
                }
                }

                SecurityRoleBL securityRoleBL = new SecurityRoleBL();
                SecurityRole role = securityRoleBL.GetByName(securityRoleShortName, callerContext);
                if (role != null && role.Id > 0)
                {
                    Collection<SecurityUser> users = GetUsersInRole(role.Id);
                    if (users != null && users.Count > 0)
                    {
                        foreach (SecurityUser user in users)
                        {
                            securityUsers.Add(user);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("SecurityUserBL.GetUsersInRole", MDMTraceSource.DataModel);
            }
            }

            return securityUsers;
        }

        /// <summary>
        /// Searches for users with specified LoginId and Email
        /// </summary>
        /// <param name="loginId">Indicates LoginId of the user</param>
        /// <param name="email">Indicates Email of the user</param>
        /// <param name="callerContext">Caller context to indicate application and module which has called an API</param>
        /// <returns>Collection of users with specified LoginId and Email</returns>
        public SecurityUserCollection SearchUsers(String loginId, String email, CallerContext callerContext = null)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("SecurityUserBL.SearchUsers", MDMTraceSource.SecurityService, false);
            }

            SecurityUserCollection securityUsers = new SecurityUserCollection();

            try
            {
                if (Constants.TRACING_ENABLED && callerContext != null)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.SecurityService);
                }

                SecurityUserDA securityUserDA = new SecurityUserDA();
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                securityUsers = securityUserDA.SearchUsers(loginId, email, command);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("SecurityUserBL.SearchUsers", MDMTraceSource.SecurityService);
            }
            }

            return securityUsers;
        }

        #endregion

        #region CUD Methods

        /// <summary>
        /// Create new SecurityUser
        /// </summary>
        /// <param name="securityUser">Represent SecurityUser Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Creation</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        /// <exception cref="ArgumentNullException">If SecurityUser ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If SecurityUser LongName is Null or having empty String</exception>
        public OperationResult Create(SecurityUser securityUser, CallerContext callerContext)
        {
            ValidateSecurityUser(securityUser, "Create");

            securityUser.Action = ObjectAction.Create;
            return Process(securityUser, callerContext);
        }

        /// <summary>
        /// Update existing securityUser
        /// </summary>
        /// <param name="securityUser">Represent SecurityUser Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        /// <exception cref="ArgumentNullException">If SecurityUser ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If SecurityUser LongName is Null or having empty String</exception>
        public OperationResult Update(SecurityUser securityUser, CallerContext callerContext)
        {
            ValidateSecurityUser(securityUser, "Update");

            securityUser.Action = ObjectAction.Update;
            return Process(securityUser, callerContext);
        }

        /// <summary>
        /// Delete securityUser
        /// </summary>
        /// <param name="securityUser">The security user.</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>
        /// OperationResult of SecurityUser Deletion. True if delete is successful.
        /// </returns>
        /// <exception cref="ArgumentNullException">If SecurityUser Object is Null</exception>
        public OperationResult Delete(SecurityUser securityUser, CallerContext callerContext)
        {
            ValidateSecurityUser(securityUser, "Delete");

            securityUser.Action = ObjectAction.Delete;
            return Process(securityUser, callerContext);
        }

        /// <summary>
        /// Process (Create/Update/Delete) given user.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="callerContext">Context indicating who trigger the change</param>
        /// <returns>
        /// OperationResult status of current process
        /// </returns>
        /// <exception cref="MDMOperationException">112069;Users cannot be null.;SecurityUserBL.Process;Process</exception>
        public OperationResultCollection Process(SecurityUserCollection users, CallerContext callerContext)
        {
            OperationResultCollection userProcessOperationResults = new OperationResultCollection();

            #region Validations

            if (users == null)
            {
                throw new MDMOperationException("112069", "Users cannot be null.", "SecurityUserBL.Process", String.Empty, "Process");
            }

            #endregion Validations

            foreach (SecurityUser user in users)
            {
                OperationResult or = Process(user, callerContext);
                if (or != null)
                {
                    userProcessOperationResults.Add(or);
                }
            }
            return userProcessOperationResults;
        }

        /// <summary>
        /// Processes the security user using the claims identity provided.
        /// </summary>
        /// <param name="claimsIdentity">The claims identity of the logged in user</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityUser process</returns>
        public OperationResult ProcessUsingClaimsIdentity(ClaimsIdentity claimsIdentity, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("SecurityUserBL.ProcessUsingClaimsIdentity", MDMTraceSource.SecurityService, false);
            }

            OperationResult operationResult = new OperationResult();

            String userName = String.Empty;
            var durationHelper = new DurationHelper(DateTime.Now);
            try
            {
                ValidateClaimsIdentity(claimsIdentity);

                userName = claimsIdentity.Name;

                Collection<SecurityRole> securityRolesForUser = GetSecurityRolesForUser(claimsIdentity, callerContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Completed building security roles for {0} in {1} ms.", userName,
                        durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.SecurityService);
                }

                ValidateUserRoles(userName, securityRolesForUser);

                SecurityUser securityUser = GetUser(userName, callerContext);
                if (securityUser != null && securityUser.Id > 0)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, 
                            String.Format("User {0} exists in the system. Attempting to update user details.", userName), MDMTraceSource.SecurityService);
                    }

                    UpdateSecurityUser(securityUser, claimsIdentity, securityRolesForUser);
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, 
                            String.Format("User {0} does not exist in the system. Attempting to create user", userName), MDMTraceSource.SecurityService);
                    }

                    securityUser = CreateSecurityUser(claimsIdentity, securityRolesForUser);
                }

                operationResult = Process(securityUser, callerContext);

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.None || 
                    operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    // Add the updated SecurityUser as return value
                    operationResult.AddReturnValue(securityUser);

                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("User {0} processed successfully", userName), MDMTraceSource.SecurityService);
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Completed processing user information for {0} in {1} ms.", userName,
                        durationHelper.GetDurationInMilliseconds(DateTime.Now)));

                    MDMTraceHelper.StopTraceActivity("SecurityUserBL.ProcessUsingClaimsIdentity", MDMTraceSource.SecurityService);
            }
            }

            return operationResult;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects">Data model object collection</param>
        /// <param name="operationResultCollection">Data model operation result collection</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            SecurityUserCollection securityUsers = iDataModelObjects as SecurityUserCollection;

            if (securityUsers != null && securityUsers.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int32 securityUserToBeCreated = -1;

                foreach (SecurityUser securityUser in securityUsers)
                {
                    DataModelOperationResult securityUserOperationResult = new DataModelOperationResult(securityUser.Id, securityUser.SecurityUserLogin, securityUser.ExternalId, securityUser.ReferenceId);

                    if (String.IsNullOrEmpty(securityUserOperationResult.ExternalId))
                    {
                        securityUserOperationResult.ExternalId = securityUser.SecurityUserLogin;
                    }

                    if (securityUser.Id < 1)
                    {
                        securityUser.Id = securityUserToBeCreated;
                        securityUserOperationResult.Id = securityUserToBeCreated;
                        securityUserToBeCreated--;
                    }

                    operationResultCollection.Add(securityUserOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ValidateInputParameters(iDataModelObjects as SecurityUserCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            SecurityUserCollection securityUsers = iDataModelObjects as SecurityUserCollection;

            if (securityUsers != null)
            {
                LoadOriginalSecurityUsers(securityUsers, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillSecurityUsers(iDataModelObjects as SecurityUserCollection, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CompareAndMerge(iDataModelObjects as SecurityUserCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResults">The operation results.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            SecurityUserCollection securityUsers = iDataModelObjects as SecurityUserCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (securityUsers != null && securityUsers.Count > 0)
            {
                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = "SecurityUserBL.Process";
                }

                #region Perform security user updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    new SecurityUserDA().Process(securityUsers, operationResults, callerContext.ProgramName, userName, command);
                    transactionScope.Complete();
                }

                UserPreferencesCollection userPreferences = new UserPreferencesCollection();
                
                foreach (SecurityUser securityUser in securityUsers)
                {
                    userPreferences.Add(securityUser.UserPreference);
                }

                IDataModelOperationResultCollection userPreferencesOperationResults = DataModelProcessOrchestrator.Validate(_iUserPreferencesManager, userPreferences, callerContext);

                DataModelProcessOrchestrator.Process(_iUserPreferencesManager, userPreferences, userPreferencesOperationResults, callerContext);
                
                LocalizeErrors(operationResults, callerContext);

               //TODO: Merge User preference operation result to parent operation result

                #endregion
            }
        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
        }

        #endregion

        #endregion

        #region Private Methods

        #region Claims Based Authentication

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="securityRoleCollection"></param>
        /// <returns></returns>
        private SecurityUser CreateSecurityUser(ClaimsIdentity claimsIdentity, Collection<SecurityRole> securityRoleCollection)
        {
            SecurityUser securityUser = new SecurityUser();
            securityUser.Action = ObjectAction.Create;
            securityUser.SecurityUserLogin = claimsIdentity.Name;
            securityUser.AuthenticationType = AuthenticationType.Claims;

            // Reset disabled flag
            securityUser.Disabled = false;
            securityUser.IsSystemCreatedUser = true;

            BuildSecurityUserUsingClaimsIdentity(securityUser, claimsIdentity, securityRoleCollection);

            return securityUser;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="claimsIdentity"></param>
        /// <param name="updatedSecurityRoleCollection"></param>
        private void UpdateSecurityUser(SecurityUser securityUser, ClaimsIdentity claimsIdentity, Collection<SecurityRole> updatedSecurityRoleCollection)
        {
            securityUser.Action = ObjectAction.Update;

            // Copy Private role as it is not available through claims.
            SecurityRole userPrivateRole = GetUserPrivateRole(securityUser.SecurityUserLogin, securityUser.Roles);
            if (userPrivateRole != null)
            {
                updatedSecurityRoleCollection.Add(userPrivateRole);
            }

            // Only flush and fill update is supported.
            securityUser.Roles.Clear();

            BuildSecurityUserUsingClaimsIdentity(securityUser, claimsIdentity, updatedSecurityRoleCollection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="securityRoleCollection"></param>
        /// <returns></returns>
        private SecurityRole GetUserPrivateRole(String userName, SecurityRoleCollection securityRoleCollection)
        {
            foreach (SecurityRole securityRole in securityRoleCollection)
            {
                if (securityRole.IsPrivateRole || securityRole.Name.Equals(userName)) //TODO: Need to check why IsPrivateRole is always false.
                {
                    return securityRole;
            }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="claimsIdentity"></param>
        /// <param name="securityRoleCollection"></param>
        private void BuildSecurityUserUsingClaimsIdentity(SecurityUser securityUser, ClaimsIdentity claimsIdentity, Collection<SecurityRole> securityRoleCollection)
        {
            ClaimTypesMapping claimTypesMapping = AppConfigurationHelper.GetClaimTypesMapping();
            if (claimTypesMapping != null)
            {
                securityUser.Smtp = GetClaimValueBasedOnClaimTypeMapping(claimsIdentity, claimTypesMapping.EmailAddress);

                String firstName = GetClaimValueBasedOnClaimTypeMapping(claimsIdentity, claimTypesMapping.FirstName);
                securityUser.FirstName = String.IsNullOrEmpty(firstName) ? 
                                        GetClaimValueBasedOnClaimTypeMapping(claimsIdentity, claimTypesMapping.DisplayName) : firstName;

                securityUser.LastName = GetClaimValueBasedOnClaimTypeMapping(claimsIdentity, claimTypesMapping.LastName);
                securityUser.Initials = GetClaimValueBasedOnClaimTypeMapping(claimsIdentity, claimTypesMapping.Initials);

                String managerId = GetClaimValueBasedOnClaimTypeMapping(claimsIdentity, claimTypesMapping.ManagerId);
                securityUser.ManagerId = String.IsNullOrWhiteSpace(managerId) ? -1 : ValueTypeHelper.ConvertToInt32(managerId);
            }

            // Build roles
            foreach (SecurityRole securityRole in securityRoleCollection)
            {
                securityUser.Roles.Add(securityRole);
            }

            securityUser.AuthenticationToken = ClaimsBasedAuthenticationHelper.ConvertClaimsToXML(claimsIdentity);
        }

        /// <summary>
        /// Returns the collection of security roles based on the role names specified.
        /// </summary>
        private Collection<SecurityRole> GetSecurityRolesForUser(ClaimsIdentity claimsIdentity, CallerContext callerContext)
        {
            Collection<SecurityRole> securityRolesForUser = null;

            Collection<String> mdmRoleNamesForUser = ClaimsBasedAuthenticationHelper.GetUserRoleNamesBasedOnClaims(claimsIdentity);            
            if (mdmRoleNamesForUser != null && mdmRoleNamesForUser.Count > 0)
            {
                SecurityRoleBL securityRoleBL = new SecurityRoleBL();
                SecurityRoleCollection securityRoleCollection = securityRoleBL.GetAll(callerContext);
                securityRolesForUser = FilterSecurityRoles(securityRoleCollection, mdmRoleNamesForUser);
            }

            return securityRolesForUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRoleCollection"></param>
        /// <param name="mdmRoleNamesForUser"></param>
        /// <returns></returns>
        private Collection<SecurityRole> FilterSecurityRoles(SecurityRoleCollection securityRoleCollection, Collection<String> mdmRoleNamesForUser)
        {
            Collection<SecurityRole> securityRolesForUser = new Collection<SecurityRole>();

            SecurityRole securityRole = null;
            foreach (String roleName in mdmRoleNamesForUser)
            {
                securityRole = GetSecurityRoleByName(securityRoleCollection, roleName);
                if (securityRole != null)
                {
                    securityRolesForUser.Add(securityRole);
                }
                else
                {
                    ClaimsBasedAuthenticationHelper.WriteLog(TraceEventType.Warning, String.Format("Unable to find security role with name: {0}", roleName));
                }
            }
            return securityRolesForUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRoleCollection"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        private SecurityRole GetSecurityRoleByName(SecurityRoleCollection securityRoleCollection, String roleName)
        {
            foreach (SecurityRole securityRole in securityRoleCollection)
            {
                if (securityRole.Name.Equals(roleName))
                {
                    return securityRole;
            }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsIdentity"></param>
        private void ValidateClaimsIdentity(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new MDMOperationException("113159", "ClaimsIdentity cannot be null.", "SecurityUserBL.ProcessUsingClaimsIdentity", String.Empty, "ProcessUsingClaimsIdentity");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="securityRolesForUser"></param>
        private void ValidateUserRoles(String userName, Collection<SecurityRole> securityRolesForUser)
        {
            if (securityRolesForUser == null || securityRolesForUser.Count == 0)
            {
                String message = String.Format("User {0} does not have any roles assigned", userName);
                ClaimsBasedAuthenticationHelper.WriteLog(TraceEventType.Error, message);
                throw new MDMOperationException("113160", message, "SecurityUserBL.ProcessUsingClaimsIdentity", String.Empty, "ProcessUsingClaimsIdentity");
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        private SecurityUserCollection GetUsers(CallerContext callerContext, Int32 roleId = 0, Int32 userId = 0, SecurityUserType userType = SecurityUserType.Unknown, String userLogin = "")
        {
            SecurityUserDA securityUserDA = new SecurityUserDA();
            String userName = String.Empty;
            SecurityUserCollection users = null;

            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            users = securityUserDA.Get(userName, 0, userId);

            LoadSecurityRoles(users);

            return users;
        }

        /// <summary>
        /// Get Security User details.
        /// </summary>
        /// <param name="userLogins">Indicates collection of user LoginId</param>
        /// <param name="userIds">Indicates collection of userIds</param>        
        /// <param name="includeDisabledUsers">Indicates whether disabled users should be ignored or not</param>
        /// <returns>Returns security user collection</returns> 
        private SecurityUserCollection GetUsers(Collection<String> userLogins, Collection<Int32> userIds, Boolean includeDisabledUsers = false)
        {
            if ((userLogins == null || userLogins.Count == 0) && (userIds == null || userIds.Count == 0))
            {
                return null;
            }

            SecurityUserDA securityUserDA = new SecurityUserDA();
            SecurityUserCollection securityUsers = securityUserDA.GetUsers(userLogins, userIds, includeDisabledUsers);

            LoadSecurityRoles(securityUsers);

            return securityUsers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        private void LoadSecurityRoles(SecurityUserCollection users)
        {
            if (users != null)
            {
                SecurityRoleBL securityRoleBL = new SecurityRoleBL();

                foreach (SecurityUser user in users)
                {
                    user.Roles = securityRoleBL.GetUserRoles(user.Id, user.SecurityUserLogin);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUsers"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalSecurityUsers(SecurityUserCollection securityUsers, CallerContext callerContext)
        {
            SecurityUserCollection originalSecurityUsers = GetAll(callerContext);

            if (originalSecurityUsers != null && originalSecurityUsers.Count > 0)
            {
                foreach (SecurityUser securityUser in securityUsers)
                {
                    securityUser.OriginalSecurityUser = originalSecurityUsers.Get(securityUser.SecurityUserLogin);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUsers"></param>
        /// <param name="callerContext"></param>
        private void FillSecurityUsers(SecurityUserCollection securityUsers, CallerContext callerContext)
        {
            SecurityRoleCollection allSecurityRoles = null;

            foreach (SecurityUser securityUser in securityUsers)
            {
                if (securityUser == null)
                {
                    continue;
                }

                if (securityUser.OriginalSecurityUser != null)
                {
                    securityUser.Id = securityUser.OriginalSecurityUser.Id;

                    // Fix for the Bug 193349
                    // Don't overwrite passwords for created or updated users
                    // Currently all updated users are marked with ObjectAction.Create action. Also OriginalSecurityUsers are retrieved without any passwords from DB.
                    // This leads to the behavior where every securityUser with Create/Update action gets updated every time even when there are no changes.     
                    if ((securityUser.Action != ObjectAction.Create && securityUser.Action != ObjectAction.Update)
                        || String.IsNullOrEmpty(securityUser.Password))
                    {
                    securityUser.Password = securityUser.OriginalSecurityUser.Password;
                }
                }

                if (String.IsNullOrWhiteSpace(securityUser.Name))
                {
                    securityUser.Name = securityUser.FirstName;
                }

                if (String.IsNullOrWhiteSpace(securityUser.LongName))
                {
                    securityUser.LongName = securityUser.LastName;
                }

                #region Fill Security Role

                SecurityRoleCollection securityRoles = securityUser.Roles ?? new SecurityRoleCollection();

                //Populate the role information for the incoming security user
                if (securityRoles.Count > 0)
                {
                    foreach (SecurityRole securityRole in securityRoles)
                    {
                        if (securityRole.Id < 1)
                        {
                            //Initialize allSecurityRoles if null
                            if (allSecurityRoles == null)
                            {
                                allSecurityRoles = new SecurityRoleBL().GetAll(callerContext);
                            }

                            SecurityRole originalSecurityRole = allSecurityRoles.Get(securityRole.Name);

                            //Populate the missing information
                            if (originalSecurityRole != null)
                            {
                                securityRole.Id = originalSecurityRole.Id;
                                securityRole.LongName = originalSecurityRole.LongName;
                            }
                        }
                    }
                }

                //Add internal roles and private role if present in original security user
                SecurityUser originalSecurityUser = securityUser.OriginalSecurityUser;

                if (originalSecurityUser != null && originalSecurityUser.Roles != null && originalSecurityUser.Roles.Count > 0)
                {
                    foreach (SecurityRole originalRole in originalSecurityUser.Roles)
                    {
                        if (InternalObjectCollection.SecurityRoleNames.Contains(originalRole.Name.ToLowerInvariant()))
                {
                            securityRoles.Add(originalRole);
                        }
                    }

                    SecurityRole privateRole = originalSecurityUser.Roles.GetPrivateRole();

                    //Security role user mapping is always flush and fill. Since system internally maintains private role,API has to populate in case of update scenario.
                    if (privateRole != null)
                    {
                        securityRoles.Add(privateRole);
                    }
                }

                securityUser.Roles = securityRoles;

                #endregion
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// Process (Create/Update/Delete) given users.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="callerContext">Context indicating who trigger the change</param>
        /// <returns>
        /// OperationResult status of current process
        /// </returns>
        /// <exception cref="MDMOperationException">
        /// 111846;CallerContext cannot be null.;SecurityUserBL.Process;Process
        /// or
        /// 112069;Users cannot be null.;SecurityUserBL.Process;Process
        /// or
        /// </exception>
        private OperationResult Process(SecurityUser user, CallerContext callerContext)
        {
            OperationResult userProcessOperationResult = new OperationResult();

            var diagActivity = new DiagnosticActivity();


            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "SecurityUserBL.Process", String.Empty, "Process");
            }

            if (user == null)
            {
                throw new MDMOperationException("112069", "Users cannot be null.", "SecurityUserBL.Process", String.Empty, "Process");
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "SecurityUserBL.Process";
            }

            #endregion Validations

            if (callerContext.TraceSettings.IsTracingEnabled)
            {
                diagActivity.Start();
            }

            //Here we need to call GetSecurityPrincipal() every time in method 
            //because we can't put it in constructor for this class due to some flow issue
            GetSecurityPrincipal();

            String userName = String.Empty;
            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            try
            {
                SecurityUserDA securityUserDA = new SecurityUserDA();
                userProcessOperationResult = securityUserDA.ProcessUser(user, callerContext.ProgramName, userName, callerContext);

                if (userProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    var cache = CacheFactory.GetCache();

                    if (cache != null)
                    {
                        //Removing the cache key so that the next time anyone access that object will fetch from DB and have all the latest updates. 
                        var key = CacheKeyGenerator.GetSecurityPrincipalCacheKey(userName);
                        if (!cache.Remove(key))
                        {
                            diagActivity.LogWarning("Not able to remove Security Principal object from Cache. UserName : " + userName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }
            finally
            {
                if (callerContext.TraceSettings.IsTracingEnabled)
                {
                    diagActivity.Stop();
            }
            }

            return userProcessOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUsers"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(SecurityUserCollection securityUsers, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (SecurityUser deltaSecurityUser in securityUsers)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaSecurityUser.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaSecurityUser.Action == ObjectAction.Read || deltaSecurityUser.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                ISecurityUser origSecurityUser = deltaSecurityUser.OriginalSecurityUser;

                if (origSecurityUser != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaSecurityUser.Action != ObjectAction.Delete)
                    {
                        origSecurityUser.MergeDelta(deltaSecurityUser, callerContext, false);
                    }
                }
                else
                {
                    if (deltaSecurityUser.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113684", "Delete action is invalid for the security user: {0}", new Object[] { deltaSecurityUser.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if ((deltaSecurityUser.AuthenticationType == AuthenticationType.Forms) && String.IsNullOrWhiteSpace(deltaSecurityUser.Password))
                        {
                            AddOperationResult(operationResult, "113642", "Password is missing for login name: {0}", new Object[] { deltaSecurityUser.SecurityUserLogin }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        //If original object is not found then set Action as Create always.
                        deltaSecurityUser.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaSecurityUser.Action;

                foreach (SecurityRole securityRole in deltaSecurityUser.Roles)
                {
                    if (securityRole.Id <= 0)
                    {
                        // TODO: Add message code for error
                        AddOperationResult(operationResult, "", "Security role '{0}' does not exist in the system. Cannot be processed further.", new Object[] {securityRole.Name}, OperationResultType.Error, TraceEventType.Warning, callerContext);
                    }
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            //if (_securityPrincipal == null)
            //{
            //    _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            //}
        }

        /// <summary>
        /// if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        /// <param name="callerContext">CallerContext</param>
        private void LocalizeErrors(DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                foreach (Error error in operationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResults(DataModelOperationResultCollection operationResults, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            callerContext = callerContext ?? new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DataModelImport, "SecurityUserBL");

            if (parameters != null && parameters.Length > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResult(IDataModelOperationResult operationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            callerContext = callerContext ?? new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DataModelImport, "SecurityUserBL");

            if (parameters != null && parameters.Length > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="claimTypesMapping"></param>
        /// <returns></returns>
        private String GetClaimValueBasedOnClaimTypeMapping(ClaimsIdentity claimsIdentity, String claimTypesMapping)
        {
            Claim claim = claimsIdentity.FindFirst(claimTypesMapping);
            if (claim != null)
            {
                return claim.Value;
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="methodName"></param>
        private void ValidateSecurityUser(SecurityUser securityUser, String methodName)
        {
            if (securityUser == null)
            {
                throw new MDMOperationException("112058", "Users cannot be null.", "SecurityUserBL" + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// Validates the input parameters.
        /// </summary>
        /// <param name="securityUsers">The security users.</param>
        /// <param name="operationResults">The operation results.</param>
        /// <param name="callerContext">The caller context.</param>
        private void ValidateInputParameters(SecurityUserCollection securityUsers, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (securityUsers == null || securityUsers.Count < 1)
            {
                AddOperationResults(operationResults, "113586", "Security users are not available or empty.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                Collection<String> internalSecurityUserNames = InternalObjectCollection.SecurityUserNames;
                Collection<String> userLoginNames = new Collection<String>();

                Regex loginNameRegex = new Regex(@"^[a-zA-Z0-9А-Яа-яЁё.@-_]+$");

                foreach (SecurityUser user in securityUsers)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(user.ReferenceId);

                    if (internalSecurityUserNames.Contains(user.SecurityUserLogin.ToLowerInvariant()))
                    {
                        AddOperationResult(operationResult, "113704", String.Format("'{0}' is an internal user. Hence will not be processed further.", user.SecurityUserLogin), new Object[] { user.SecurityUserLogin }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                    }
                    if (String.IsNullOrWhiteSpace(user.FirstName))
                    {
                        AddOperationResult(operationResult, "113659", "User first name cannot be empty", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    
                    if (String.IsNullOrWhiteSpace(user.LastName))
                    {
                        AddOperationResult(operationResult, "113661", "User last name cannot be empty", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }

                    if (String.IsNullOrWhiteSpace(user.Smtp))
                    {
                        AddOperationResult(operationResult, "113660", "User email address cannot be empty.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else if (!Utility.IsValidEmail(user.Smtp))
                    {
                        AddOperationResult(operationResult, "113933", "Unable to proceed. Enter a valid e-mail address, example: email.Id@company.com", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }

                    if (String.IsNullOrWhiteSpace(user.SecurityUserLogin))
                    {
                        AddOperationResult(operationResult, "113641", "User login name cannot be empty.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (!loginNameRegex.IsMatch(user.SecurityUserLogin))
                    {
                        AddOperationResult(operationResult, "110821", "Please specify a valid Login ( Cannot contain spaces, special Characters except @ )", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                        else
                        {
                            String userLoginName = user.SecurityUserLogin.ToLowerInvariant();

                            if (userLoginNames.Contains(userLoginName))
                            {
                                AddOperationResult(operationResult, "113897", "User login '{0}' already exists.", new Object[] { user.SecurityUserLogin }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else
                            {
                                userLoginNames.Add(userLoginName);
                            }
                        }
                    }

                    SecurityRoleCollection securityRoles = user.Roles;

                    if (securityRoles != null && securityRoles.Count > 0)
                    {
                        foreach (SecurityRole securityRole in securityRoles)
                        {
                            if (InternalObjectCollection.SecurityRoleNames.Contains(securityRole.Name.ToLowerInvariant()))
                    {
                                AddOperationResult(operationResult, "113705", "'{0}' is an internal security role. Hence will not be processed further.", new Object[] { securityRole.Name }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                            }
                        }
                    }
                }
            }

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", "CallerContext cannot be null.", null, OperationResultType.Error, TraceEventType.Error, null);
            }
        }

        #endregion

        #endregion Private methods

        #endregion
    }
}