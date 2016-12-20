using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Core;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get permissions related information.
    /// </summary>
    public interface IPermission 
    {
        #region Properties
        /// <summary>
        /// Property denoting the id of the object
        /// </summary>
        Int64 ObjectId { get; set; }
        
        /// <summary>
        /// Property denoting the set of permissions
        /// </summary>
        Collection<UserAction> PermissionSet { get; set; }
       
        /// <summary>
        /// Property denoting the type of the permission
        /// </summary>
        PermissionType PermissionType { get; set; }
        
        /// <summary>
        /// Property denoting the context sequence
        /// </summary>
        Int32 Sequence { get; set; }
        #endregion

        #region Methods

         /// <summary>
        /// Gets  permission context
        /// </summary>
        /// <returns> Permission context interface</returns>
          IPermissionContext GetPermissionContext();

        /// <summary>
        /// Sets permission context
        /// </summary>
        /// <param name="iPermissionContext"> Permission context interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed  permission context is null</exception>
        void SetPermissionContext(IPermissionContext iPermissionContext);

        /// <summary>
        /// Gets parent permission
        /// </summary>
        /// <returns> Parent permission interface</returns>
         IPermission GetParentPermission();

         /// <summary>
        /// Sets parent permission
        /// </summary>
        /// <param name="iPermission"> Parent Permission interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed  permission is null</exception>
        void SetParentPermission(IPermission iPermission);

        #endregion

    }
}
