using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.PermissionManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.Interfaces;
    using MDM.Core.Exceptions;
    using MDM.MessageManager.Business;

    /// <summary>
    /// Business logic for permission BL
    /// </summary>
    public class PermissionBL
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
        /// Field which denotes whether AVB security enabled or not
        /// </summary>
        private Boolean _isAVBSEnabled = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes Permission BL
        /// </summary>
        public PermissionBL()
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
        /// Gets attribute value based entity permissions
        /// </summary>
        /// <param name="entity">Entity for which permissions needs to be determined</param>
        /// <param name="entityOperationResult">The object results of the operation</param>
        /// <param name="iEntityManager">Entity Manager instance used to access Entity methods</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Permission object having permission set</returns>
        /// <exception cref="MDMOperationException">Thrown when Entity object is not available or user is not having permission</exception>
        public Permission GetValueBasedEntityPermission(Entity entity, EntityOperationResult entityOperationResult, IEntityManager iEntityManager, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("PermissionBL.GetValueBasedEntityPermission", false);

            Permission entityPermission = null;
            PermissionCollection rolePermissions = null;
            SecurityPermissionDefinitionCollection securityPermissionDefinitions = null;

            try
            {
                if (this._isAVBSEnabled && entity.EntityTypeId != 6)
                {
                    #region Validate Parameters

                    if (entity == null)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111835", false, callerContext);
                        throw new MDMOperationException("111835", _localeMessage.Message, "PermissionManager", String.Empty, "GetValueBasedEntityPermission");//Value based permissions cannot be determined. Please provide the entity for which permission needs to be determined.
                    }

                    #endregion

                    if (entityOperationResult == null)
                    {
                        entityOperationResult = new EntityOperationResult(entity.Id, entity.ExternalId, entity.LongName);
                    }

                    //Get security permission definitions for the entity
                    securityPermissionDefinitions = GetEntityPermissionDefinitions(entity, callerContext);

                    if (securityPermissionDefinitions != null && securityPermissionDefinitions.Count > 0)
                    {
                        #region Permission Match

                        rolePermissions = GetMatchedValuesPermissions(entity, securityPermissionDefinitions, entityOperationResult, iEntityManager, callerContext);

                        #endregion

                        #region Permission Merge

                        entityPermission = MergeRolePermissions(rolePermissions);

                        #endregion

                        if (entityPermission == null || entityPermission.PermissionSet.Count < 1)
                        {
                            //There is no permission available..
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("There is no View/Edit permission available for the entity '{0}'", entity.LongName));

                            //Populate operation result with error
                            _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111836", false, callerContext); //View/Edit permissions are not available for this product view
                            entityOperationResult.AddOperationResult("111836", _localeMessage.Message, OperationResultType.Error);
                        }
                        else if (entityPermission.PermissionSet.Contains(UserAction.View) && !entityPermission.PermissionSet.Contains(UserAction.Update))
                        {
                            //There is no edit permission..
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("There is no Edit permission available for the entity '{0}'", entity.LongName));

                            //Add an info message..
                            _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111841", false, callerContext); //Edit permission is not available for this product view
                            entityOperationResult.AddOperationResult("111841", _localeMessage.Message, OperationResultType.Information);
                        }
                    }
                    else
                    {
                        //There is no security permission definitions available..
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Security permissions are not defined for the entity '{0}'. User is not having any permission.", entity.LongName));

                        //Populate operation result with error
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111836", false, callerContext); //View/Edit permissions are not available for this product view
                        entityOperationResult.AddOperationResult("111836", _localeMessage.Message, OperationResultType.Error);
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute Value Based security is disabled. Skipping value based security..");

                    //Add both view and edit permissions
                    entityPermission = new Permission();
                    entityPermission.PermissionSet.Add(UserAction.View);
                    entityPermission.PermissionSet.Add(UserAction.Update);
                    entityPermission.PermissionSet.Add(UserAction.WorkflowActions);
                    entityPermission.PermissionSet.Add(UserAction.Reclassify);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("PermissionBL.GetValueBasedEntityPermission");
            }

            return entityPermission;
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

        private SecurityPermissionDefinitionCollection GetEntityPermissionDefinitions(Entity entity, CallerContext callerContext)
        {
            SecurityPermissionDefinitionCollection securityPermissionDefinitions = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Getting Security Permission Definitions for entity '{0}'", entity.LongName));

            if (_securityPrincipal != null)
            {
                //Prepare application context
                ApplicationContext applicationContext = new ApplicationContext();
                applicationContext.OrganizationId = entity.OrganizationId;
                applicationContext.OrganizationLongName = entity.OrganizationLongName;
                applicationContext.ContainerId = entity.ContainerId;
                applicationContext.ContainerLongName = entity.ContainerLongName;
                applicationContext.UserId = _securityPrincipal.CurrentUserId;
                applicationContext.UserName = _securityPrincipal.CurrentUserName;

                SecurityPermissionDefinitionBL securityPermissionDefinitionManager = new SecurityPermissionDefinitionBL();
                securityPermissionDefinitions = securityPermissionDefinitionManager.Get(applicationContext, callerContext);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Completed Security Permission Definitions get.");
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Security Permission Definitions cannot be fetched. Security Principal is not available.");
            }

            return securityPermissionDefinitions;
        }

        private PermissionCollection GetMatchedValuesPermissions(Entity entity, SecurityPermissionDefinitionCollection securityPermissionDefinitions, EntityOperationResult entityOperationResult, IEntityManager iEntityManager, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Matching definitions with the attribute values..");

            Permission rolePermission = null;
            PermissionCollection rolePermissions = new PermissionCollection();

            foreach (SecurityPermissionDefinition securityPermissionDefinition in securityPermissionDefinitions)
            {
                //Get attribute Id..
                Int32 attributeId = securityPermissionDefinition.ApplicationContext.AttributeId;

                //Get defined values
                Collection<String> definitionValues = securityPermissionDefinition.PermissionValues;

                if (attributeId < 1 && definitionValues != null && definitionValues.Contains("[rsall]"))
                {
                    //This is 'All Data Access' permission definition. 
                    //No need to check for attribute values.. 
                    //Populate the role permission with the permissions set of this definition
                    rolePermission = new Permission();
                    rolePermission.PermissionSet = securityPermissionDefinition.PermissionSet;

                    rolePermissions.Add(rolePermission);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Having 'All Data Access' definition. Setting all permissions to entity '{0}'. Skipping other definitions processing..", entity.LongName));

                    //No need to continue with other definitions.. coming out of the loop
                    break;
                }
                else if (attributeId > 0)
                {
                    #region Ensure Attribute

                    //Ensure attribute
                    IAttribute attributeInContext = GetAttribute(entity, attributeId, iEntityManager, callerContext);

                    #endregion

                    #region Match Values and Prepare Permissions

                    if (attributeInContext != null)
                    {
                        Boolean isMatchFound = false;

                        if (definitionValues != null)
                        {
                            if (definitionValues.Contains("[rsall]"))
                            {
                                isMatchFound = true;
                            }
                            else
                            {
                                //Get attribute values..
                                IValueCollection iValues = attributeInContext.GetCurrentValuesInvariant();

                                if (iValues != null && iValues.Count > 0)
                                {
                                    foreach (IValue iValue in iValues)
                                    {
                                        if (definitionValues.Contains(iValue.GetStringValue().Trim().ToLowerInvariant()))
                                        {
                                            isMatchFound = true;

                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    //No values available. Hence no permissions.
                                    if (Constants.TRACING_ENABLED)
                                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute in context '{0}' is not having any values for entity '{1}'. Permission set of the definition '{2}' is not being considered.", attributeInContext.LongName, entity.LongName, securityPermissionDefinition.LongName));
                                }
                            }

                            if (isMatchFound)
                            {
                                //Match found..
                                //Populate the role permission with the permissions set for this definition and skip other values.
                                rolePermission = new Permission();
                                rolePermission.PermissionSet = securityPermissionDefinition.PermissionSet;

                                rolePermissions.Add(rolePermission);
                            }
                            else
                            {
                                //There is no match..
                                //Log a message
                                if (Constants.TRACING_ENABLED)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute in context '{0}' value is not having a match with the definition. Permission set of the definition '{1}' is not being considered.", attributeInContext.LongName, securityPermissionDefinition.LongName));
                            }
                        }
                        else
                        {
                            //Attribute values are not defined. Skipping this security definition..
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Security Permission Definition '{0}' is not valid. Attribute values are not defined. Skipping this definition..", securityPermissionDefinition.LongName));
                        }
                    }

                    #endregion
                }
                else
                {
                    //Attribute Id is not available. Skipping this security definition..
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Security Permission Definition '{0}' is not valid. Attribute Id is not available. Skipping this definition..", securityPermissionDefinition.LongName));
                }
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Matching of definitions with the attribute values completed.");

            return rolePermissions;
        }

        private Permission MergeRolePermissions(PermissionCollection rolePermissions)
        {
            Permission entityPermission = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Merging role permissions..");

            if (rolePermissions.Count > 0)
            {
                entityPermission = new Permission();

                foreach (Permission permission in rolePermissions)
                {
                    foreach (UserAction userAction in permission.PermissionSet)
                    {
                        if (!entityPermission.PermissionSet.Contains(userAction))
                        {
                            entityPermission.PermissionSet.Add(userAction);
                        }
                    }
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Merging of role permissions completed.");
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "There are no permissions available for merge.");
            }

            return entityPermission;
        }

        private IAttribute GetAttribute(Entity entity, Int32 attributeId, IEntityManager iEntityManager, CallerContext callerContext)
        {
            IAttribute attributeInContext = null;

            try
            {
                attributeInContext = entity.GetAttribute(attributeId);

                if (attributeInContext == null)
                {
                    var attributeIdList = new Collection<Int32>();
                    attributeIdList.Add(attributeId);

                    var entityContext = new EntityContext();

                    entityContext.LoadEntityProperties = false;
                    entityContext.ContainerId = entity.ContainerId;
                    entityContext.EntityTypeId = entity.EntityTypeId;
                    entityContext.CategoryId = entity.CategoryId;
                    entityContext.Locale = entity.Locale;
                    entityContext.DataLocales = entity.EntityContext.DataLocales;
                    entityContext.LoadAttributes = true;
                    entityContext.AttributeIdList = attributeIdList;
                    entityContext.LoadAttributeModels = false;

                    var entityGetOptions = new EntityGetOptions
                    {
                        PublishEvents = false,
                        ApplyAVS = false,
                        ApplySecurity = false
                    };

                    entityGetOptions.FillOptions.SetAllEntityPropertiesLevelFillOptionsTo(false);

                    Entity freshEntity = iEntityManager.Get(entity.Id, entityContext, entityGetOptions, callerContext);

                    if (freshEntity != null)
                    {
                        attributeInContext = freshEntity.GetAttribute(attributeId);
                    }
                }

                if (attributeInContext == null)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute in context '{0}' is not mapped to the Container '{1}' and Entity Type '{2}'.", attributeId, entity.ContainerLongName, entity.EntityTypeLongName));
                }
            }
            catch (Exception ex)
            {
                //Failed to find attribute..
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred while ensuring security attribute. Error: {0}", ex.Message));
            }

            return attributeInContext;
        }

        #endregion

        #endregion
    }
}