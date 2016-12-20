using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Transactions;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects;
    using MDM.AdminManager.Data;
    using MDM.Interfaces;
    using MDM.BusinessObjects.DataModel;
    using MDM.ConfigurationManager.Business;
    using MDM.MessageManager.Business;
    using MDM.BufferManager;
    
    /// <summary>
    /// Specifies operations for security role
    /// </summary>
    public class SecurityRoleBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Filed denoting DataLocale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        #endregion Fields 

        #region Constructors

        public SecurityRoleBL()
        {
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
        /// <param name="getPermissionSetOnly"></param>
        /// <param name="countFrom"></param>
        /// <param name="countTo"></param>
        /// <param name="sortColumn"></param>
        /// <param name="searchColumn"></param>
        /// <param name="searchParameter"></param>
        /// <param name="userLogin"></param>
        /// <param name="displaySystemRole"></param>
        /// <returns></returns>
        public Collection<SecurityRole> GetAllRoles(Int32 userType, Char getPermissionSetOnly, Int32 countFrom, Int32 countTo, String sortColumn, String searchColumn, String searchParameter, String userLogin, Boolean displaySystemRole)
        {
            Collection<SecurityRole> securityRoleCollection = null;

            SecurityRoleDA securityRoleDA = new SecurityRoleDA();
            securityRoleCollection = securityRoleDA.GetAllRoles(userType, getPermissionSetOnly, countFrom, countTo, sortColumn, searchColumn, searchParameter, userLogin, displaySystemRole);

            return securityRoleCollection;
        }

        /// <summary>
        /// Returns roles to which user belongs
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="userLoginName">Login Name of the user</param>
        /// <returns>Collection of security roles</returns>
        public SecurityRoleCollection GetUserRoles(Int32 userId, String userLoginName)
        {
            SecurityRoleCollection userRoles = null;
            SecurityRoleBufferManager securityRoleBufferManager = new SecurityRoleBufferManager();

            //Check user roles exist in the cache
            userRoles = securityRoleBufferManager.FindUserRoles(userLoginName);

            if (userRoles == null)
            {
                //Failed to get from cache..
                //Get from DB
                SecurityRoleDA securityRoleDA = new SecurityRoleDA();
                userRoles = securityRoleDA.GetUserRoles(userId, userLoginName);

                if (userRoles != null && userRoles.Count > 0)
                {
                    //cache the roles..
                    securityRoleBufferManager.UpdateUserRoles(userRoles, userLoginName, 3);
                }
            }

            return userRoles;
        }

        /// <summary>
        /// Get available Roles based on the Role Name. 
        /// </summary>
        /// <param name="securityRoleShortName">Short name of role.</param>
        /// <param name="callerContext">>Context indicating which application is making API call</param>
        /// <returns>Returns Security role object with details.</returns>
        public SecurityRole GetByName(String securityRoleShortName, CallerContext callerContext)
        {
            MDMTraceHelper.InitializeTraceSource();
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("SecurityRoleBL.GetByName", MDMTraceSource.DataModel, false);

            #region Parameter Validation
            ValidateCallerContext(callerContext, "GetByName");
            if (String.IsNullOrEmpty(securityRoleShortName))
            {
                String errorMessage = "Security Role ShortName cannot be empty.";
                throw new MDMOperationException("112075", errorMessage, "AdminManager.SecurityRoleBL", String.Empty, "GetByName", MDMTraceSource.DataModel);
            }
            #endregion

            SecurityRole securityRole = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Security role Name : " + securityRoleShortName, MDMTraceSource.DataModel);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);
                }

                Collection<SecurityRole> securityRoles = this.GetAllRoles(0, 'N', 0, 10000, "LongName", String.Empty, String.Empty, String.Empty, false);

                if (securityRoles != null && securityRoles.Count > 0)
                {
                    foreach (SecurityRole tempSecurityRole in securityRoles)
                    {
                        if (tempSecurityRole.Name.Equals(securityRoleShortName))
                        {
                            securityRole = tempSecurityRole;
                            break;
                        }
                    }
                }
            }
            finally 
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("SecurityRoleBL.GetByName", MDMTraceSource.DataModel);
            }
            return securityRole;
        }

        /// <summary>
        /// Get all the roles from system
        /// </summary>
        /// <param name="userType">User type</param>
        /// <param name="getPermissionSetOnly">Get only permission set</param>
        /// <param name="getSystemRole">Flag indicating is to show system role</param>
        /// <returns>Role collection</returns>
        public SecurityRoleCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.StartTraceActivity("AdminManager.SecurityRoleBL.GetAllRoles", false);

            #region Parameter Validation

            if (callerContext == null)
            {
                throw new MDMOperationException("111823", String.Empty, "SecurityRoleBL", String.Empty, "GetAllRoles");//CallerContext is null or empty
            }

            #endregion Parameter Validation

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Starting : AdminManager.SecurityRoleBL.GetAllRoles");

            SecurityRoleCollection roles = this.GetRoles(callerContext);
            
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : AdminManager.SecurityRoleBL.GetAllRoles");
            MDMTraceHelper.StopTraceActivity("AdminManager.SecurityRoleBL.GetAllRoles");

            return roles;
        }

        /// <summary>
        /// Get all the roles from system
        /// </summary>
        /// <param name="userType">User type</param>
        /// <param name="getPermissionSetOnly">Get only permission set</param>
        /// <param name="getSystemRole">Flag indicating is to show system role</param>
        /// <returns>Role collection</returns>
        public SecurityRole GetById(Int32 roleId, CallerContext callerContext)
        {
            #region Parameter Validation

            if (callerContext == null)
            {
                throw new MDMOperationException("111823", String.Empty, "SecurityRoleBL", String.Empty, "GetByRoleId");//CallerContext is null or empty
            }

            if (roleId < 1)
            {
                throw new MDMOperationException("112083", String.Empty, "SecurityRoleBL", String.Empty, "GetByRoleId");//roleId cannot be less than 1
            }

            #endregion Parameter Validation

            MDMTraceHelper.StartTraceActivity("AdminManager.SecurityRoleBL.GetAllRoles", false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Starting : AdminManager.SecurityRoleBL.GetByRoleId");

            SecurityRoleCollection roles = this.GetRoles(callerContext,roleId);
            SecurityRole role = null;

            if (roles != null && roles.Count > 0)
            {
                role = roles.FirstOrDefault();
            }

            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : AdminManager.SecurityRoleBL.GetByRoleId");
            MDMTraceHelper.StopTraceActivity("AdminManager.SecurityRoleBL.GetAllRoles");

            return role;
        }

        /// <summary>
        /// Get roles based on given criteria. Giving no parameter value will return all the roles
        /// </summary>
        /// <param name="callerContext">Context of application which made the API call</param>
        /// <param name="roleId">Id of role to search for</param>
        /// <param name="userType">Get roles based on SecurityUserType</param>
        /// <param name="roleShortName">Role name to search</param>
        /// <param name="returnPrivateRole">Indicates if to return private role</param>
        /// <param name="permissionSet">Indicates if to return permission set</param>
        /// <returns>Role collection</returns>
        public SecurityRoleCollection GetRoles(CallerContext callerContext, Int32 roleId = 0, SecurityUserType userType = SecurityUserType.Unknown, String roleShortName = "", Boolean returnPrivateRole = false, Boolean permissionSet = false)
        {
            SecurityRoleDA securityRoleDA = new SecurityRoleDA();
            String userName = String.Empty;
            SecurityRoleCollection roles = null;

            //Here we need to call GetSecurityPrincipal() every time in method 
            //because we can't put it in constructor for this class due to some flow issue
            GetSecurityPrincipal();

            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            roles = securityRoleDA.Get(userName, roleId, userType, roleShortName, returnPrivateRole, permissionSet);

            return roles;
        }

        #endregion

        #region CUD Methods

        /// <summary>
        /// Create new SecurityRole
        /// </summary>
        /// <param name="securityRole">Represent SecurityRole Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Creation</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        /// <exception cref="ArgumentNullException">If SecurityRole ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If SecurityRole LongName is Null or having empty String</exception>
        public OperationResult Create(SecurityRole securityRole, CallerContext callerContext)
        {
            this.ValidateSecurityRole(securityRole, "Create");

            securityRole.Action = Core.ObjectAction.Create;
            return this.Process(securityRole, callerContext);
        }

        /// <summary>
        /// Update existing securityRole
        /// </summary>
        /// <param name="securityRole">Represent SecurityRole Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Updating</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        /// <exception cref="ArgumentNullException">If SecurityRole ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If SecurityRole LongName is Null or having empty String</exception>
        public OperationResult Update(SecurityRole securityRole, CallerContext callerContext)
        {
            this.ValidateSecurityRole(securityRole, "Update");

            securityRole.Action = Core.ObjectAction.Update;
            return this.Process(securityRole, callerContext);
        }

        /// <summary>
        /// Delete securityRole
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of SecurityRole Deletion</returns>
        /// <returns>True if SecurityRole Creation is Successful</returns>
        /// <exception cref="ArgumentNullException">If SecurityRole Object is Null</exception>
        public OperationResult Delete(SecurityRole securityRole, CallerContext callerContext)
        {
            this.ValidateSecurityRole(securityRole, "Delete");

            securityRole.Action = Core.ObjectAction.Delete;
            return this.Process(securityRole, callerContext);
        }

        /// <summary>
        /// Process (Create/Update/Delete) given role.
        /// </summary>
        /// <param name="role">Role to be processed</param>
        /// <param name="programName">Name of program which made change</param>
        /// <param name="callerContext">Context indicating who trigger the change</param>
        /// <returns>OperationResult status of current process</returns>
        public OperationResultCollection Process(SecurityRoleCollection roles, CallerContext callerContext)
        {
            OperationResultCollection roleProcessOperationResults = new OperationResultCollection();

            #region Validations

            if (roles == null)
            {
                throw new MDMOperationException("112069", "Roles cannot be null.", "SecurityRoleBL.Process", String.Empty, "Process");
            }
            
            #endregion Validations

            foreach (SecurityRole role in roles)
            {
                OperationResult or = this.Process(role, callerContext);
                if(or!= null)
                {
                    roleProcessOperationResults.Add(or);
                }
            }
            return roleProcessOperationResults;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            SecurityRoleCollection securityRoles = iDataModelObjects as SecurityRoleCollection;

            if (securityRoles != null && securityRoles.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int32 securityRoleToBeCreated = -1;

                foreach (SecurityRole securityRole in securityRoles)
                {
                    DataModelOperationResult securityRoleOperationResult = new DataModelOperationResult(securityRole.Id, securityRole.LongName, securityRole.ExternalId, securityRole.ReferenceId);

                    if (String.IsNullOrEmpty(securityRoleOperationResult.ExternalId))
                    {
                        securityRoleOperationResult.ExternalId = securityRole.Name;
                    }

                    if (securityRole.Id < 1)
                    {
                        securityRole.Id = securityRoleToBeCreated;
                        securityRoleOperationResult.Id = securityRoleToBeCreated;
                        securityRoleToBeCreated--;
                    }

                    operationResultCollection.Add(securityRoleOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ValidateInputParameters(iDataModelObjects as SecurityRoleCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            SecurityRoleCollection securityRoles = iDataModelObjects as SecurityRoleCollection;

            if (securityRoles != null)
            {
                LoadOriginalSecurityRoles(securityRoles, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillSecurityRoles(iDataModelObjects as SecurityRoleCollection, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CompareAndMerge(iDataModelObjects as SecurityRoleCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            SecurityRoleCollection securityRoles = iDataModelObjects as SecurityRoleCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (securityRoles.Count > 0)
            {
                String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = "SecurityRoleBL.Process";
                }

                #region Perform security role updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    new SecurityRoleDA().Process(securityRoles, operationResults, callerContext.ProgramName, userName, command);
                    transactionScope.Complete();
                }

                LocalizeErrors(operationResults, callerContext);

                #endregion
            }
        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
        }

        #endregion

        #endregion

        #region Private Methods

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
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
            if (parameters != null && parameters.Count() > 0)
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
            if (parameters != null && parameters.Count() > 0)
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

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRoles"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalSecurityRoles(SecurityRoleCollection securityRoles, CallerContext callerContext)
        {
            SecurityRoleCollection originalSecurityRoles = GetAll(callerContext);

            if (originalSecurityRoles != null && originalSecurityRoles.Count > 0)
            {
                foreach (SecurityRole securityRole in securityRoles)
                {
                    securityRole.OriginalSecurityRole = originalSecurityRoles.Get(securityRole.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRoles"></param>
        /// <param name="callerContext"></param>
        private void FillSecurityRoles(SecurityRoleCollection securityRoles, CallerContext callerContext)
        {
            foreach (SecurityRole securityRole in securityRoles)
            {
                if (securityRole.Id < 1)
                {
                    securityRole.Id = (securityRole.OriginalSecurityRole != null) ? securityRole.OriginalSecurityRole.Id : securityRole.Id;
                    securityRole.IsSystemRole = (securityRole.OriginalSecurityRole != null) && securityRole.OriginalSecurityRole.IsSystemRole;
                }

                SecurityUserCollection securityUsers = securityRole.Users;

                if (securityUsers != null && securityUsers.Count > 0)
                {
                    foreach (SecurityUser securityUser in securityUsers)
                    {
                        SecurityUser originalSecurityUser = null;

                        if (securityUser.Id < 1)
                        {
                            if (securityRole.OriginalSecurityRole != null && securityRole.OriginalSecurityRole.Users != null && securityRole.OriginalSecurityRole.Users.Count > 0)
                            {
                                originalSecurityUser = securityRole.OriginalSecurityRole.Users.Get(securityUser.SecurityUserLogin);
                            }
                            else
                            {
                                originalSecurityUser = new SecurityUserBL().GetUser(securityUser.SecurityUserLogin, callerContext);
                            }

                            if (originalSecurityUser != null)
                            {
                                securityUser.Id = originalSecurityUser.Id;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// Process (Create/Update/Delete) given roles.
        /// </summary>
        /// <param name="roles">Roles to be processed</param>
        /// <param name="callerContext">Context indicating who trigger the change</param>
        /// <returns>OperationResult status of current process</returns>
        private OperationResult Process(SecurityRole role, CallerContext callerContext)
        {
            OperationResult roleProcessOperationResult = new OperationResult();

            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "SecurityRoleBL.Process", String.Empty, "Process");
            }

            if (role == null)
            {
                throw new MDMOperationException("112069", "Roles cannot be null.", "SecurityRoleBL.Process", String.Empty, "Process");
            }

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "SecurityRoleBL.Process";
            }

            #endregion Validations

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
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    SecurityRoleDA securityRoleDA = new SecurityRoleDA();
                    roleProcessOperationResult = securityRoleDA.ProcessRole(role, callerContext.ProgramName, userName, callerContext);
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }

            return roleProcessOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRoles"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(SecurityRoleCollection securityRoles, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (SecurityRole deltaSecurityRole in securityRoles)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaSecurityRole.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaSecurityRole.Action == ObjectAction.Read || deltaSecurityRole.Action == ObjectAction.Ignore)
                    continue;

                ISecurityRole origSecurityRole = deltaSecurityRole.OriginalSecurityRole;

                if (origSecurityRole != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaSecurityRole.Action != ObjectAction.Delete)
                    {
                        origSecurityRole.MergeDelta(deltaSecurityRole, callerContext, false);
                    }
                }
                else
                {
                    if (deltaSecurityRole.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113647", String.Empty, new Object[] { deltaSecurityRole.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        //If original object is not found then set Action as Create always.
                        deltaSecurityRole.Action = ObjectAction.Create;
                    }
                }
                operationResult.PerformedAction = deltaSecurityRole.Action;
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRole"></param>
        /// <param name="methodName"></param>
        private void ValidateSecurityRole(SecurityRole securityRole, String methodName)
        {
            if (securityRole == null)
            {
                throw new MDMOperationException("112069", "Roles cannot be null.", "SecurityRoleBL" + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="methodName"></param>
		private void ValidateCallerContext(CallerContext callerContext, String methodName)
        {
            String errorMessage = String.Empty;

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "AdminManager.SecurityRoleBL", String.Empty, methodName, MDMTraceSource.DataModel);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityRoles"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(SecurityRoleCollection securityRoles, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (securityRoles == null || securityRoles.Count < 1)
            {
                AddOperationResults(operationResults, "113587", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            Collection<String> roleNames = new Collection<String>();
            Collection<String> internalSecurityRoleNames = MDM.Core.DataModel.InternalObjectCollection.SecurityRoleNames;
            foreach (SecurityRole securityRole in securityRoles)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(securityRole.ReferenceId);
                String securityRoleName = securityRole.Name;

                if (internalSecurityRoleNames.Contains(securityRoleName.ToLowerInvariant()))
                {
                    AddOperationResult(operationResult, "113705", String.Format("'{0}' is an internal security role. Hence will not be processed further.", securityRoleName), new Object[] { securityRoleName }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                }
                else if (roleNames.Contains(securityRoleName))
                {
                    AddOperationResult(operationResult, "114005", String.Format("Security role with the specified short name: {0} already exists. You must specify a different short name.", securityRoleName), new Object[] { securityRoleName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
                else
                {
                    roleNames.Add(securityRoleName);
                }
            }
        }

        #endregion

        #endregion

        #endregion
    }
}