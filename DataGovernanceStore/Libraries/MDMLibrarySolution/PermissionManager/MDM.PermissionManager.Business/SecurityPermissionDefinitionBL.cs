using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MDM.PermissionManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.AdminManager.Business;
    using MDM.PermissionManager.Data;
    using MDM.MessageManager.Business;
    using MDM.ConfigurationManager.Business;

    /// <summary>
    /// Business Logic for Security Permission Definition
    /// </summary>
    public class SecurityPermissionDefinitionBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting instance of Locale Message BL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting instance of security permission DA
        /// </summary>
        private SecurityPermissionDefinitionDA _securityPermissionDefinitionDA = new SecurityPermissionDefinitionDA();

        /// <summary>
        /// Field denoting instance of securityPermissionDefinitionBufferManager
        /// </summary>
        private PermissionBufferManager _securityPermissionDefinitionBufferManager = new PermissionBufferManager();

        /// <summary>
        /// Field which denotes whether AVB security enabled or not
        /// </summary>
        private Boolean _isAVBSEnabled = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public SecurityPermissionDefinitionBL()
        {
            GetSecurityPrincipal();

            try
            {
                // Get AppConfig which specify whether Attribute value based security is enabled for entity or not
                String strIsAVBSEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.AttributeValueBasedSecurity.Enabled");
                Boolean.TryParse(strIsAVBSEnabled, out _isAVBSEnabled);
            }
            catch
            {
                //Ignore
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get the Security Permission Definition Collection based on securityRole
        /// </summary>
        /// <param name="securityRole">Indicates the security role</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns Security Permission Definition Collection</returns> 
        /// <exception cref="MDMOperationException">Thrown if security role is not available</exception>
        public SecurityPermissionDefinitionCollection Get(SecurityRole securityRole, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("PermissionManager.Get", false);

            SecurityPermissionDefinitionCollection securityPermissionDefinitions = null;

            try
            {
                if (this._isAVBSEnabled)
                {
                    #region Parameter Validation

                    if (securityRole == null || securityRole.Id < 1)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111830", false, callerContext);
                        throw new MDMOperationException("111830", _localeMessage.Message, "PermissionManager", String.Empty, "Get");//Role Id is not available. Please provide the role id for which permissions are required.
                    }

                    #endregion

                    if (isTracingEnabled)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("The Security Permission Definitions get request is for the Role '{0}'", securityRole.LongName));
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get from cache if available..");
                    }

                    securityPermissionDefinitions = _securityPermissionDefinitionBufferManager.FindSecurityPermissionDefinitions(securityRole);

                    if (securityPermissionDefinitions == null)
                    {
                        if (isTracingEnabled)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Security Permission Definitions are not available in cache.");
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting Security Permission Definitions from database..");
                        }

                        //Get command
                        DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);
                        securityPermissionDefinitions = _securityPermissionDefinitionDA.Get(securityRole.Id, command);

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Setting Security Permission Definitions in cache.");
                        _securityPermissionDefinitionBufferManager.UpdateSecurityPermissionDefinitions(securityPermissionDefinitions, securityRole.Id, 3);
                    }
                    else
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Security Permission Definitions are being fetched from cache.");
                    }
                }
                else
                {
                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute Value Based security is disabled. Skipping Security Permission Definitions get..");
                }
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("PermissionManager.Get");
            }

            return securityPermissionDefinitions;
        }

        /// <summary>
        /// Get the Security Permission Definition based on application context
        /// </summary>
        /// <param name="applicationContext">Indicates instance of application context</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns Security Permission Definitions with the best match to the requested context.</returns>
        /// <exception cref="MDMOperationException">Thrown when context parameters are not provided</exception>
        public SecurityPermissionDefinitionCollection Get(ApplicationContext applicationContext, CallerContext callerContext)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("PermissionManager.Get", false);

            SecurityPermissionDefinitionCollection securityPermissionDefinitions = new SecurityPermissionDefinitionCollection();

            try
            {
                if (this._isAVBSEnabled)
                {
                    #region Parameter Validation

                    ValidateApplicationContext(applicationContext, callerContext);

                    #endregion

                    //Check for context RoleId..
                    if (applicationContext.RoleId > 0)
                    {
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("The context for Security Permission Definitions get request is - Org: '{0}' Container: '{1}'  Role: '{2}'", applicationContext.OrganizationName, applicationContext.ContainerName, applicationContext.RoleName));

                        SecurityRole securityRole = new SecurityRole(applicationContext.RoleId, applicationContext.RoleName, applicationContext.RoleName);

                        securityPermissionDefinitions = GetContextSecurityDefinition(securityRole, applicationContext, callerContext);
                    }
                    else if (applicationContext.UserId > 0)
                    {
                        //Request is for user permission definitions
                        //As per the assumptions, Permission definitions are not set at the user level
                        //So load the permission definitions for all the roles of the user and cache them
                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("The context for Security Permission Definitions get request is - Org: '{0}' Container: '{1}' User: '{2}'", applicationContext.OrganizationName, applicationContext.ContainerName, applicationContext.UserName));

                        SecurityPermissionDefinitionCollection roleSecurityPermissionDefinitions = null;

                        #region Get User Roles

                        //Get roles for this user..
                        SecurityRoleCollection userRoles = null;

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Getting roles mapped to user '{0}'", applicationContext.UserName));

                        if (_securityPrincipal != null)
                        {
                            SecurityRoleBL securityRoleBL = new SecurityRoleBL();
                            userRoles = securityRoleBL.GetUserRoles(_securityPrincipal.CurrentUserId, _securityPrincipal.CurrentUserName);
                        }

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting roles mapped to user completed.");

                        if (userRoles == null || userRoles.Count < 1)
                        {
                            _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111834", false, callerContext);
                            throw new MDMOperationException("111834", _localeMessage.Message, "PermissionManager", String.Empty, "Get");//Permission definitions cannot be fetched. Unable to get the roles for the user or user is not associated with any role.
                        }

                        #endregion

                        foreach (SecurityRole securityRole in userRoles)
                        {
                            roleSecurityPermissionDefinitions = GetContextSecurityDefinition(securityRole, applicationContext, callerContext);

                            if (roleSecurityPermissionDefinitions != null && roleSecurityPermissionDefinitions.Count > 0)
                            {
                                foreach (SecurityPermissionDefinition securityPermissionDefinition in roleSecurityPermissionDefinitions)
                                {
                                    securityPermissionDefinitions.Add(securityPermissionDefinition);
                                }
                            }
                        }
                    }

                    //Sort according to the context weightage so that 'All Data Access' permissions comes on top if available..
                    //It helps in further logic of figuring out of permissions and as soon as it finds 'All Data Access' role skips further definitions
                    //calculation and assigns all permissions.
                    securityPermissionDefinitions.OrderBy(s => s.ContextWeightage);
                }
                else
                {
                    SecurityPermissionDefinition securityPermissionDefinition = new SecurityPermissionDefinition();

                    securityPermissionDefinition.PermissionValues.Add("[rsall]");
                    securityPermissionDefinition.PermissionSet.Add(UserAction.View);
                    securityPermissionDefinition.PermissionSet.Add(UserAction.Update);
                    securityPermissionDefinition.PermissionSet.Add(UserAction.WorkflowActions);
                    securityPermissionDefinition.PermissionSet.Add(UserAction.Reclassify);

                    securityPermissionDefinitions.Add(securityPermissionDefinition);

                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute Value Based security is disabled. Adding all permissions manually. Skipping Security Permission Definitions get.");
                }
            }
            finally
            {
                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("PermissionManager.Get");
            }

            return securityPermissionDefinitions;
        }

        #endregion

        #region Private Methods

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private void ValidateApplicationContext(ApplicationContext applicationContext, CallerContext callerContext)
        {
            if (applicationContext == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111831", false, callerContext);
                throw new MDMOperationException("111831", _localeMessage.Message, "PermissionManager", String.Empty, "Get");//Application Context is not available. Please provide the application context for which permissions are required.
            }

            //TODO:: At present, not necessary to check the organization id, But need to re-look this.
            //if (applicationContext.OrganizationId < 1)
            //{
            //    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Organization Id is not available. Please provide the organization Id for which permission definitions are required.");

            //    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111832", false, callerContext);
            //    throw new MDMOperationException("111832", _localeMessage.Message, "PermissionManager", String.Empty, "Get");//Organization Id is not available. Please provide the organization id for which permissions are required.
            //}

            if (applicationContext.RoleId < 1)
            {
                if (applicationContext.UserId < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111833", false, callerContext);
                    throw new MDMOperationException("111833", _localeMessage.Message, "PermissionManager", String.Empty, "Get");//Role Id and User Id are not available. Please provide either role Id or user Id for which permission definitions are required.
                }
            }
        }

        private SecurityPermissionDefinitionCollection GetContextSecurityDefinition(SecurityRole securityRole, ApplicationContext applicationContext, CallerContext callerContext)
        {
            SecurityPermissionDefinitionCollection securityPermissionDefinitions = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Getting security definitions for role '{0}'..", securityRole.LongName));

            securityPermissionDefinitions = Get(securityRole, callerContext);

            if (securityPermissionDefinitions != null && securityPermissionDefinitions.Count > 0)
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Security Permission Definitions are fetched for role '{0}'.", securityRole.LongName));

                securityPermissionDefinitions = GetNearestMatchedSecurityDefinitions(securityPermissionDefinitions, applicationContext);
            }
            else
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Security Permission Definitions for requested role '{0}] are not available. Skipping other context parameters..", securityRole.LongName));
            }

            return securityPermissionDefinitions;
        }

        private SecurityPermissionDefinitionCollection GetNearestMatchedSecurityDefinitions(SecurityPermissionDefinitionCollection securityPermissionDefinitions, ApplicationContext applicationContext)
        {
            SecurityPermissionDefinitionCollection matchedSecurityPermissionDefinitions = null;
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Finding the nearest matched permission definition for the requested context parameters..");

            IEnumerable<SecurityPermissionDefinition> nearestMatchedDefinitions = securityPermissionDefinitions.Where(s => (s.ApplicationContext.OrganizationId == applicationContext.OrganizationId || s.ApplicationContext.OrganizationId == 0) &&
                                                                                                                           (s.ApplicationContext.ContainerId == applicationContext.ContainerId || s.ApplicationContext.ContainerId == 0));
            if (nearestMatchedDefinitions != null && nearestMatchedDefinitions.Count() > 0)
            {
                IEnumerable<SecurityPermissionDefinition> groupedDefinitions = nearestMatchedDefinitions.OrderByDescending(p => p.ContextWeightage).GroupBy(p => p.ContextWeightage).First();

                if (groupedDefinitions != null && groupedDefinitions.Count() > 0)
                {
                    matchedSecurityPermissionDefinitions = new SecurityPermissionDefinitionCollection();

                    foreach (SecurityPermissionDefinition securityPermissionDefinition in groupedDefinitions)
                    {
                        matchedSecurityPermissionDefinitions.Add(securityPermissionDefinition);
                    }
                }

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Found the nearest matched permission definition for the requested context parameters.");
            }
            else
            {
                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "There are no permission definitions matching for the requested context parameters.");
            }

            return matchedSecurityPermissionDefinitions;
        }

        #endregion

        #endregion
    }
}