using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.AdminManager.Data;
    using MDM.CacheManager.Business;

    /// <summary>
    /// Specifies the business operations for attribute model
    /// </summary>
    public class DataSecurityBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// Specifies roles of current user
        /// </summary>
        private SecurityRoleCollection _currentUserRoles = null;

        /// <summary>
        /// Specifies permissions of current user
        /// </summary>
        private Collection<DataSecurity> _currentUserPermissions = new Collection<DataSecurity>();

        /// <summary>
        /// field denoting cache manager of distributed cache
        /// </summary>
        private IDistributedCache _distributedCacheManager = null;

        /// <summary>
        /// field denoting cache manager of ICache
        /// </summary>
        private ICache _iCacheManager = null;

        /// <summary>
        /// Field denotes whether the Distributed Cache with notification has been enabled.
        /// </summary>
        private Boolean _isDistributedCacheWithNotificationEnabled = false;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public DataSecurityBL()
        {
            InitializeCacheObject();
            GetSecurityPrincipal();
            this._currentUserRoles = GetUserRoles();
        }

        /// <summary>
        /// Constructor which takes the user as well as the System to load the security principal
        /// </summary>
        /// <param name="user">The user creating this instance</param>
        /// <param name="system">Indicates which system requested this instance</param>
        public DataSecurityBL(String user, MDMCenterSystem system)
            : base(user, system)
        {
            InitializeCacheObject();
            GetSecurityPrincipal(user, system);
            this._currentUserRoles = GetUserRoles();
        }

        /// <summary>
        /// Constructor which takes the security principal if its already present in the system 
        /// </summary>
        /// <param name="securityPrincipalInstance">The user specific security principal of this instance</param>
        public DataSecurityBL(SecurityPrincipal securityPrincipalInstance)
        {
            InitializeCacheObject();
            _securityPrincipal = securityPrincipalInstance;
            SecurityPrincipalHelper.ValidateSecurityPrincipal(_securityPrincipal);
            this._currentUserRoles = GetUserRoles();
        }

        /// <summary>
        /// Constructor which loads permissions of the user specified by security principal by default
        /// <para>
        /// Preferred to use this constructor for attribute model load
        /// </para>
        /// </summary>
        /// <param name="securityPrincipalInstance">The user specific security principal of this instance</param>
        /// <param name="permissionContext">Context for permission</param>
        public DataSecurityBL(SecurityPrincipal securityPrincipalInstance, PermissionContext permissionContext)
        {
            InitializeCacheObject();
            _securityPrincipal = securityPrincipalInstance;
            SecurityPrincipalHelper.ValidateSecurityPrincipal(_securityPrincipal);
            this._currentUserRoles = GetUserRoles();
            LoadPermissions(permissionContext);
        }

        #endregion

        #region Properties
        
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Loads all object permissions in the requested context
        /// </summary>
        /// <param name="context">Context under which permissions needs to be loaded</param>
        public void LoadPermissions(PermissionContext context)
        {
            if (context != null)
            {
                #region Load User Permission From Cache

                if (context.UserId > 0)
                {
                    if (LoadUserPermissionFromCache(context.UserId))   //User Permission Loaded from cache
                    {
                        return;
                    }
                }

                #endregion

                #region Load User Permission

                //ASSUMPTION:: There are no permissions defined at the User level..
                //Permissions must be set at role level

                //Check for context RoleId..
                if (context.RoleId > 0)
                {
                    //Yes.. RoleId has been provided. The request is for that particular role id.
                    //Load role permission and cache it.
                    LoadRolePermissions(context);
                }
                else if (context.UserId > 0)
                {
                    //Request is for user permissions
                    //As per the assumptions, Permissions are not set at the user level
                    //So load the permissions for all the roles of the user and cache them

                    //Load permissions for each role of the user and cache them
                    foreach (SecurityRole role in _currentUserRoles)
                    {
                        context.RoleId = role.Id;
                        LoadRolePermissions(context);
                    }

                    //Reset role Id
                    context.RoleId = 0;
                }
                #endregion

                #region Update User permission into cache

                if (context.UserId > 0)
                {
                    UpdateUserPermissionCache(context.UserId);
                }
                #endregion
            }
        }

        /// <summary>
        /// Gets all object permissions in the requested context
        /// </summary>
        /// <param name="context">Context under which permissions needs to be loaded</param>
        /// <returns>Collections of permissions</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed permission context is null</exception>
        /// <exception cref="ArgumentException">Thrown when permission context is not having role id or user id for which permissions are required or user id is not associated with any role</exception>
        public PermissionCollection GetPermissions(PermissionContext context)
        {
            //Validate Permission Context
            ValidatePermissionContext(context);

            PermissionCollection permissionCollection = null;

            //ASSUMPTION:: There are no permissions defined at the User level..
            //Permissions must be set at role level

            //Check for role id..
            if (context.RoleId > 0)
            {
                //Yes.. role id is available. Request is for role permissions
                //Load role permissions
                permissionCollection = LoadRolePermissions(context);
            }
            else
            {
                //Request is for user permissions
                //As per the assumptions, Permissions are not set at the user level
                //So get the permissions for all the roles of the user and merge them to get the User permissions

                Collection<PermissionCollection> rolePermissionCollection = new Collection<PermissionCollection>();
                foreach (SecurityRole role in _currentUserRoles)
                {
                    context.RoleId = role.Id;
                    PermissionCollection rolePermissions = LoadRolePermissions(context);

                    if (rolePermissions != null && rolePermissions.Count > 0)
                        rolePermissionCollection.Add(rolePermissions);
                }

                //Merge role permissions
                permissionCollection = MergeRolePermissions(rolePermissionCollection);
            }

            return permissionCollection;
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
        public Permission GetMDMObjectPermission(Int64 mdmObjectId, Int32 mdmObjectTypeId, String mdmObjectType, PermissionContext context)
        {
            //Validate Permission Context
            ValidatePermissionContext(context);

            //Validate object details
            //if (mdmObjectId < 1)
            //    throw new ArgumentException("MDM Object Id is not available. Please provide the object id for which permissions are required.");

            //Validate object details
            if (String.IsNullOrWhiteSpace(mdmObjectType))
                throw new ArgumentException("MDM Object Type is not available. Please provide the object type for which permissions are required.");

            Permission permission = null;

            //ASSUMPTION:: There are no permissions defined at the User level..
            //Permissions must be set at role level

            //Check for role id..
            if (context.RoleId > 0)
            {
                //Yes.. role id is available. Request is for the available role permissions
                //Load role permissions for the requested MDM object
                permission = LoadMDMObjectPermission(mdmObjectId, mdmObjectTypeId, mdmObjectType, context);
            }
            else
            {
                //Request is for user permissions
                //As per the assumptions, Permissions are not set at the user level
                //So get the permissions for all the roles of the user for the requested MDM object and merge them to get the User permissions

                PermissionCollection rolePermissions = new PermissionCollection();
                foreach (SecurityRole role in _currentUserRoles)
                {
                    //Set the context role id
                    context.RoleId = role.Id;
                    Permission rolePermission = LoadMDMObjectPermission(mdmObjectId, mdmObjectTypeId, mdmObjectType, context);

                    //Reset the context role id to zero
                    context.RoleId = 0;

                    if (rolePermission != null)
                        rolePermissions.Add(rolePermission);
                }

                //Merge role permissions
                permission = MergeObjectPermissions(rolePermissions);
            }

            return permission;
        }

        /// <summary>
        /// Gets the permission set for the requested attribute id in the provided context
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which permissions are required</param>
        /// <param name="attributeObjectTypeId">Id of the attribute object type for which permission is required</param>
        /// <param name="context">Context under which permissions needs to be loaded</param>
        /// <returns>Permission set</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed permission context is null</exception>
        /// <exception cref="ArgumentException">Thrown when permission context is not having role id or user id for which permissions are required or user id is not associated with any role or attribute id or attribute object type id is not provided</exception>
        public Permission GetAttributePermission(Int32 attributeId, Int32 attributeObjectTypeId, PermissionContext context)
        {
            if (attributeId < 1)
                throw new ArgumentException("Attribute Id is not available. Please provide the attribute id for which permissions are required.");

            if (attributeObjectTypeId < 1)
                throw new ArgumentException("Attribute ObjectTypeId is not available. Please provide the attribute object type id for which permissions are required.");

            Permission permission = null;

            //This will set the requested attribute id as permission context too.
            //This is required to load explicit permissions set for attribute instances
            context.AttributeId = attributeId;

            permission = GetMDMObjectPermission(attributeId, attributeObjectTypeId, "Attribute", context);

            //return
            return permission;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionContext"></param>
        /// <returns></returns>
        public Permission GetRelationshipPermission(PermissionContext permissionContext)
        {
            Permission permission = null;

            permission = GetMDMObjectPermission(0, 40, "Relationships", permissionContext);

            //return
            return permission;
        }

        /// <summary>
        /// Gets the permission set for the requested lookup as per the provided security lookup configuration
        /// </summary>
        /// <param name="lookupTableName">lookup name for which permission is requested</param>
        /// <param name="securityLookup">lookup which holds the permission set</param>
        /// <returns>Permission set</returns>
        public Permission GetLookupPermission(String lookupTableName, Lookup securityLookup)
        {
            Permission permission = null;

            if (securityLookup != null && securityLookup.Rows != null && securityLookup.Rows.Count > 0)
            {
                PermissionCollection permissions = GetFilteredLookupPermissions(securityLookup, lookupTableName);
                permission = new Permission();

                if (permissions != null && permissions.Count > 0)
                {
                    Collection<UserAction> unionOfPermissionSets = new Collection<UserAction>();
                    Boolean lookupHavingNoAccess = false;

                    #region Union of PermissionSet

                    foreach (Permission filteredPermission in permissions)
                    {
                        if (filteredPermission.PermissionSet != null && filteredPermission.PermissionSet.Count > 0)
                        {
                            foreach (UserAction action in filteredPermission.PermissionSet)
                            {
                                if (action != UserAction.None)
                                {
                                    if (!unionOfPermissionSets.Contains(action))
                                    {
                                        unionOfPermissionSets.Add(action);

                                        if (action == UserAction.All) //If All permission is there no need look forward for other permissions
                                            break;
                                    }
                                }
                                else
                                {
                                    lookupHavingNoAccess = true;
                                }
                            }
                        }
                    }

                    #endregion

                    if (unionOfPermissionSets.Count > 0)
                    {
                        permission.PermissionSet = unionOfPermissionSets;
                    }
                    else if (lookupHavingNoAccess)
                    {
                        permission.PermissionSet = new Collection<UserAction>() { UserAction.None };
                    }
                }
                else
                {
                    permission.PermissionSet = new Collection<UserAction>() { UserAction.All };
                }
            }
            
            return permission;
        }

        /// <summary>
        /// Determines whether MDM object is having the requested permission and returns the result
        /// </summary>
        /// <param name="mdmObjectId">Id of the MDM Object for which permission is required</param>
        /// <param name="mdmObjectTypeId">Type Id of the MDM Object for which permission is required</param>
        /// <param name="mdmObjectType">Type of the MDM Object for which permission is required</param>
        /// <param name="userAction">>User Action permission needs to be determined</param>
        /// <param name="context">Context under which permissions needs to be determined</param>
        /// <returns>The Boolean result for the requested permission for the requested MDM object id</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed permission context is null</exception>
        /// <exception cref="ArgumentException">Thrown when permission context is not having role id or user id for which permissions are required or user id is not associated with any role or MDM object id and type are not provided</exception>
        public Boolean HasPermission(Int64 mdmObjectId, Int32 mdmObjectTypeId, String mdmObjectType, UserAction userAction, PermissionContext context)
        {
            Boolean hasPermission = false;

            Permission permission = GetMDMObjectPermission(mdmObjectId, mdmObjectTypeId, mdmObjectType, context);

            if (permission != null && permission.PermissionSet != null && permission.PermissionSet.Contains(userAction))
                hasPermission = true;

            return hasPermission;
        }

        /// <summary>
        /// Determines whether attribute is having the requested permission and returns the result
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which permission is required</param>
        /// <param name="attributeObjectTypeId">Id of the attribute object type for which permission is required</param>
        /// <param name="userAction">User Action permission needs to be determined</param>
        /// <param name="context">Context under which permissions needs to be determined</param>
        /// <returns>The Boolean result for the requested permission for the requested attribute id</returns>
        /// <exception cref="ArgumentNullException">Thrown when passed permission context is null</exception>
        /// <exception cref="ArgumentException">Thrown when permission context is not having role id or user id for which permissions are required or user id is not associated with any role or attribute id or attribute object type id is not provided</exception>
        public Boolean HasAttributePermission(Int32 attributeId, Int32 attributeObjectTypeId, UserAction userAction, PermissionContext context)
        {
            if (attributeId < 1)
                throw new ArgumentException("Attribute Id is not available. Please provide the attribute id for which permissions are required.");

            if (attributeObjectTypeId < 1)
                throw new ArgumentException("Attribute ObjectTypeId is not available. Please provide the attribute object type id for which permissions are required.");

            Boolean hasPermission = false;

            hasPermission = HasPermission(attributeId, attributeObjectTypeId, "Attribute", userAction, context);

            return hasPermission;
        }

        #endregion

        #region Private Methods

        private void ValidatePermissionContext(PermissionContext context)
        {
            if (context == null)
                throw new ArgumentNullException("PermissionContext");

            if (context.RoleId < 1)
            {
                if (context.UserId < 1)
                    throw new ArgumentException("Permissions cannot be determined. Please provide either Role or User in the permission context.");
                else if (_currentUserRoles == null || _currentUserRoles.Count < 1)
                    throw new ArgumentException("Permissions cannot be determined. Unable to get the roles for the user or user is not associated with any role.");
            }
        }

        private PermissionCollection LoadRolePermissions(PermissionContext context)
        {
            PermissionCollection permissionCollection = null;
            DataSecurity dataSecurity=null;
            IDistributedCache cache = CacheFactory.GetDistributedCache();

            //Check in current user permissions..
            if (_currentUserPermissions != null)
            {
                dataSecurity = _currentUserPermissions.FirstOrDefault(ds => ds.RoleId == context.RoleId);
                if (dataSecurity != null)
                {
                    dataSecurity = (DataSecurity) dataSecurity.Clone();
                }
            }
            else
                _currentUserPermissions = new Collection<DataSecurity>();

            if (dataSecurity != null)
            {
                permissionCollection = dataSecurity.Permissions;
            }
            else
            {
                //Role permissions are not existing in current user permissions..
                //Try to get from cache
                if (cache != null)
                {
                    //Prepare cache key..
                    String dataSecurityCacheKey = "RS_DataSecurity_RoleId:" + context.RoleId;

                    //Get the permission collection from cache
                    Object cachedDataSecurity = cache[dataSecurityCacheKey];

                    if (cachedDataSecurity != null && cachedDataSecurity is DataSecurity)
                    {
                        dataSecurity = cachedDataSecurity as DataSecurity;

                        if (dataSecurity.Permissions != null)
                        {
                            permissionCollection = (PermissionCollection) dataSecurity.Permissions.Clone();
                        }
                    }
                    else
                    {
                        //If any cached object returned from cache IS NULL then get permissions from DB and put it into cache
                        //cache is set to expire after 30 days.
                        permissionCollection = GetPermissionsFromDB(0, String.Empty, context);

                        if (permissionCollection != null)
                        {
                            dataSecurity = new DataSecurity(context.RoleId, permissionCollection);

                            List<String> dataCacheTags = new List<String>();
                            dataCacheTags.Add("Role:" + context.RoleId);
                            
                            cache.Set(dataSecurityCacheKey, (DataSecurity)dataSecurity.Clone(), DateTime.Now.AddDays(30), dataCacheTags);
                        }
                    }
                }
                else
                {
                    //Get permissions directly from DB..
                    permissionCollection = GetPermissionsFromDB(0, String.Empty, context);

                    if (permissionCollection != null)
                        dataSecurity = new DataSecurity(context.RoleId, permissionCollection);
                }

                //Put into current user permissions
                if (dataSecurity != null)
                {
                    DataSecurity clonedDataSecurity = dataSecurity.Clone() as DataSecurity;

                    if (clonedDataSecurity != null)
                    {
                        _currentUserPermissions.Add(clonedDataSecurity);
                    }
                }
            }

            return permissionCollection;
        }

        private Permission LoadMDMObjectPermission(Int64 mdmObjectId, Int32 mdmObjectTypeId, String mdmObjectType, PermissionContext context)
        {
            Permission permission = null;
            PermissionCollection permissionCollection = LoadRolePermissions(context); //Assumes that context is loaded with role id

            if(permissionCollection!=null && permissionCollection.Count>0)
                permission = GetNearestMatchedPermission(mdmObjectId, mdmObjectTypeId, mdmObjectType, context, permissionCollection);

            return permission;
        }

        private PermissionCollection GetPermissionsFromDB(Int32 mdmObjectId, String mdmObjectType, PermissionContext context)
        {
            PermissionCollection permissionCollection = null;

            DataSecurityDA dataSecurityDA = new DataSecurityDA();
            permissionCollection = dataSecurityDA.GetPermissions(mdmObjectId, mdmObjectType, context);

            return permissionCollection;
        }

        private PermissionCollection MergeRolePermissions(Collection<PermissionCollection> rolePermissionsCollection)
        {
            PermissionCollection mergedPermissions = null;

            if (rolePermissionsCollection != null)
            {
                mergedPermissions = new PermissionCollection();

                foreach (PermissionCollection rolePermissions in rolePermissionsCollection)
                {
                    foreach (Permission permission in rolePermissions)
                    {
                        //Check whether this permission is present in the merged permission list
                        Permission perm = mergedPermissions.SingleOrDefault(p => p.ObjectId == permission.ObjectId && p.ObjectType == permission.ObjectType);

                        if (perm == null)
                        {
                            //No.. permission is not there
                            //Add into the merged list
                            mergedPermissions.Add(permission);
                        }
                        else
                        {
                            //Yes.. permission is already there.
                            //Merge this new permission with the existing one (UNION of permission)
                            foreach (UserAction userAction in permission.PermissionSet)
                            {
                                if (!perm.PermissionSet.Contains(userAction))
                                    perm.PermissionSet.Add(userAction);
                            }

                            //TODO:: Merge context..
                            //Current outgoing merged object will not be having proper context
                        }
                    }
                }
            }

            return mergedPermissions;
        }

        private Permission MergeObjectPermissions(PermissionCollection rolePermissions)
        {
            Permission mergedPermission = null;

            if (rolePermissions != null && rolePermissions.Count > 0)
            {
                foreach (Permission permission in rolePermissions)
                {
                    if (mergedPermission == null)
                        mergedPermission = permission;
                    else
                    {
                        //Merge this new permission with the existing one (UNION of permission)
                        foreach (UserAction userAction in permission.PermissionSet)
                        {
                            if (!mergedPermission.PermissionSet.Contains(userAction))
                                mergedPermission.PermissionSet.Add(userAction);
                        }
                    }

                    //TODO:: Merge context..
                    //Current outgoing merged object will not be having proper context
                }
            }

            return mergedPermission;
        }

        private Permission GetNearestMatchedPermission(Int64 mdmObjectId, Int32 mdmObjectTypeId, String mdmObjectType, PermissionContext context, PermissionCollection permissions)
        {
            Permission permission = null;

            //Find for the requested MDM object permission from the passed role permission collection
            //--------------------------------------------------
            //Step 1: Get the permissions with the exact context match or the permissions with nearest context match in the MDM hierarchy
            //Step 2: Among the permissions in the step1, get the permission with high sequence value
            //--------------------------------------------------

            //Step 1:
            IEnumerable<Permission> nearestMatchedPermissions = null;

            if (mdmObjectTypeId > 0)
            {
                nearestMatchedPermissions = permissions.Where(p => (p.ObjectId == mdmObjectId || p.ObjectId == 0) && p.ObjectTypeId == mdmObjectTypeId &&
                    //(p.Context.OrgId == context.OrgId || p.Context.OrgId == 0) && //For vendor portal Org Id is not available.. //TODO:: Discuss with Product Management
                    (p.Context.ContainerId == context.ContainerId || p.Context.ContainerId == 0) &&
                    (p.Context.CategoryId == context.CategoryId || p.Context.CategoryId == 0) &&
                    (p.Context.EntityTypeId == context.EntityTypeId || p.Context.EntityTypeId == 0) &&
                    (p.Context.RelationshipTypeId == context.RelationshipTypeId || p.Context.RelationshipTypeId == 0) &&
                    (p.Context.EntityId == context.EntityId || p.Context.EntityId == 0) &&
                    (p.Context.AttributeId == context.AttributeId || p.Context.AttributeId == 0) &&
                    (p.Context.LocaleId == context.LocaleId || p.Context.LocaleId == 0));
            }
            else
            {
                nearestMatchedPermissions = permissions.Where(p => (p.ObjectId == mdmObjectId || p.ObjectId == 0) && p.ObjectType == mdmObjectType &&
                    //(p.Context.OrgId == context.OrgId || p.Context.OrgId == 0) && //For vendor portal Org Id is not available.. //TODO:: Discuss with Product Management
                    (p.Context.ContainerId == context.ContainerId || p.Context.ContainerId == 0) &&
                    (p.Context.CategoryId == context.CategoryId || p.Context.CategoryId == 0) &&
                    (p.Context.EntityTypeId == context.EntityTypeId || p.Context.EntityTypeId == 0) &&
                    (p.Context.RelationshipTypeId == context.RelationshipTypeId || p.Context.RelationshipTypeId == 0) &&
                    (p.Context.EntityId == context.EntityId || p.Context.EntityId == 0) &&
                    (p.Context.AttributeId == context.AttributeId || p.Context.AttributeId == 0) &&
                    (p.Context.LocaleId == context.LocaleId || p.Context.LocaleId == 0));
            }

            //Step 2:
            if (nearestMatchedPermissions != null && nearestMatchedPermissions.Count() > 0)
            {
                Permission cachedPermission = nearestMatchedPermissions.OrderByDescending(p => p.Sequence).First();
                permission = new Permission(mdmObjectId, cachedPermission.ObjectTypeId, mdmObjectType, cachedPermission.PermissionSet, cachedPermission.Context, cachedPermission.Sequence);
            }

            return permission;
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private void GetSecurityPrincipal(String user, MDMCenterSystem system)
        {
            if (_securityPrincipal == null)
            {
                SecurityPrincipalBL securityPrincipalBL = new SecurityPrincipalBL();

                _securityPrincipal = securityPrincipalBL.Get(user, system);
                SecurityPrincipalHelper.ValidateSecurityPrincipal(_securityPrincipal);
            }
        }

        private SecurityRoleCollection GetUserRoles()
        {
            SecurityRoleCollection userRoles = new SecurityRoleCollection();

            if (_securityPrincipal != null)
            {
                SecurityRoleBL securityRoleBL = new SecurityRoleBL();
                userRoles = securityRoleBL.GetUserRoles(_securityPrincipal.CurrentUserId, _securityPrincipal.CurrentUserName);
            }

            return userRoles;
        }

        private void InitializeCacheObject()
        {

            this._isDistributedCacheWithNotificationEnabled = AppConfigurationHelper.IsDistributedCacheWithNotificationEnabled;

            if (_isDistributedCacheWithNotificationEnabled)
            {
                _cacheSynchronizationHelper = new CacheSynchronizationHelper();
                _iCacheManager = CacheFactory.GetCache();
            }
            else
            {
                _distributedCacheManager = CacheFactory.GetDistributedCache();
            }
        }

        private Boolean LoadUserPermissionFromCache(Int32 userId)
        {
            Boolean isLoaded = false;
            String cacheKey = CacheKeyGenerator.GetUserPermissionsCacheKey(userId);
            Object permissionObject = null;

            if (_isDistributedCacheWithNotificationEnabled)
            {
                permissionObject = _iCacheManager.Get(cacheKey);
            }
            else
            {
                permissionObject = _distributedCacheManager.Get(cacheKey);
            }

            if (permissionObject != null)
            {
                _currentUserPermissions = CloneDataSecurityCollection(permissionObject as Collection<DataSecurity>);
                isLoaded = true;
            }

            return isLoaded;
        }

        private void UpdateUserPermissionCache(Int32 userId)
        {
            String cacheKey = CacheKeyGenerator.GetUserPermissionsCacheKey(userId);
            DateTime expiryTime = DateTime.Now.AddDays(30);

            // Add user permission into cache.
            if (_isDistributedCacheWithNotificationEnabled)
            {
                _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expiryTime, false);
                _iCacheManager.Set(cacheKey, CloneDataSecurityCollection(_currentUserPermissions), expiryTime);
            }
            else
            {
                _distributedCacheManager.Set(cacheKey, CloneDataSecurityCollection(_currentUserPermissions), expiryTime);
            }
        }

        private Collection<DataSecurity> CloneDataSecurityCollection(Collection<DataSecurity> source)
        {
            Collection<DataSecurity> result = null;
            if (source != null)
            {
                result = new Collection<DataSecurity>();
                for (Int32 i = 0; i < source.Count; i++)
                {
                    result.Add((DataSecurity) source[i].Clone());
                }
            }
            return result;
        }

        private PermissionCollection GetFilteredLookupPermissions(Lookup lookup, String requestedLookupTableName)
        {
            PermissionCollection permissions = new PermissionCollection();
            Permission defaultPermissionSet = null;
            Boolean foundPermissionForUser = false;
            Boolean foundPermissionForUserRole = false;

            foreach (Row row in lookup.Rows)
            {
                Int32 userId = 0;
                Int32 localeId = 0;
                Int32 roleId = 0;

                if (lookup.Columns.Contains("User"))
                {
                    userId = ValueTypeHelper.Int32TryParse(row.GetValue("User").ToString(), 0);

                    if (userId > 0)
                    {
                        if (userId != _securityPrincipal.CurrentUserId)
                        {
                            //If UserId is configured and not equal to requested User then not required for permission calculation
                            continue;
                        }
                        else
                        {
                            foundPermissionForUser = true;
                        }
                    }
                }

                if (lookup.Columns.Contains("Role"))
                {
                    roleId = ValueTypeHelper.Int32TryParse(row.GetValue("Role").ToString(), 0);

                    if (roleId > 0)
                    {
                        if (!_securityPrincipal.UserRoleIds.Contains(roleId))
                        {
                            //If RoleId is configured and user doen't have role then not required for permission calculation
                            continue;
                        }
                        else
                        {
                            foundPermissionForUserRole = true;
                        }
                    }
                }

                if (lookup.Columns.Contains("LookupTable"))
                {
                    String lookuptableName = row.GetValue("LookupTable") != null ? row.GetValue("LookupTable").ToString() : String.Empty;

                    if (!String.IsNullOrWhiteSpace(lookuptableName) && String.Compare(lookuptableName, requestedLookupTableName, StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        //If Lookuptable name is available and not equal to requested lookupName then not require for permission calculation
                        continue;
                    }
                }

                if (lookup.Columns.Contains("Locale"))
                {
                    localeId = ValueTypeHelper.Int32TryParse(row.GetValue("Locale").ToString(), 0);
                    Int32 requestedLocaleId = (Int32)lookup.Locale;
                    
                    if (localeId > 0 && localeId != requestedLocaleId)
                    {
                        //If UserId is configured and not equal to requested User then not required for permission calculation
                        continue;
                    }
                }

                Permission permission = new Permission();
                
                if (lookup.Columns.Contains("Action"))
                {
                    String action = row.GetValue("Action") != null ? row.GetValue("Action").ToString() : String.Empty;

                    if (!String.IsNullOrWhiteSpace(action))
                    {
                        permission.PermissionSet = ValueTypeHelper.SplitStringToUserActionCollection(action, ',');
                    }
                    else
                    {
                        //No Permission for User/role/locale
                        permission.PermissionSet = new Collection<UserAction>() { UserAction.None };
                    }
                }

                if (userId != roleId)
                {
                    //UserId == 0 and RoleId == 0 then consider as default permission set
                    permissions.Add(permission);
                }
                else if (defaultPermissionSet == null)
                {
                    defaultPermissionSet = permission;
                }
            }

            if (permissions != null && permissions.Count == 0)
            {
                if ((foundPermissionForUser || foundPermissionForUserRole) && defaultPermissionSet == null)
                {
                    Permission permission = new Permission();
                    permission.PermissionSet = new Collection<UserAction>() { UserAction.None };
                    permissions.Add(permission);
                }
                else if (defaultPermissionSet != null)
                {
                    permissions.Add(defaultPermissionSet);
                }
            }

            return permissions;
        }

        #endregion

        #endregion
    }
}
