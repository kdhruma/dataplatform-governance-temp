using System;

namespace MDM.ProfileManager.Business
{
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// Represents the Profile Utility. It contains the utility methods for profile managerBL class.
    /// </summary>
    public sealed class ProfileUtility
    {
        #region Fields

        #endregion

        #region Internal Methods

        /// <summary>
        /// Validated the user permission for the requested user id.
        /// </summary>
        /// <param name="userId">Indicates the User id</param>
        /// <returns>Indicates whether the user has permission or not.</returns>
        internal static Boolean ValidateUserPermission(Int32 userId)
        {
            Permission permission = null;
            DataSecurityBL dataSecurityManager = new DataSecurityBL();
            Int32 objectTypeId = (Int32)ObjectType.Catalog;

            PermissionContext permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, userId, 0);
            permission = dataSecurityManager.GetMDMObjectPermission(0, objectTypeId, ObjectType.Catalog.ToString(), permissionContext);

            return (permission == null ? false : permission.PermissionSet.Contains(UserAction.Import));
        }

        #endregion
    }
}
