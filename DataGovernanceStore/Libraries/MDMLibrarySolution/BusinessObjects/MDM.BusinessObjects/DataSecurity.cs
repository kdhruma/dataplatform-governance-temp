using System;
using System.Xml;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Specifies DataSecurity 
    /// </summary>
    [DataContract]
    public class DataSecurity : ObjectBase, ICloneable
    {
        #region Fields

        /// <summary>
        /// The role id to which data security belongs
        /// </summary>
        private Int32 _roleId = 0;

        /// <summary>
        /// The collection of permissions for the requested role id
        /// </summary>
        private PermissionCollection _permissions = new PermissionCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Data Security class
        /// </summary>
        public DataSecurity()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Data Security class
        /// </summary>
        /// <param name="roleId">Role Id for which permissions are maintained in the Data Security class</param>
        /// <param name="permissions">Permissions for the Role</param>
        public DataSecurity(Int32 roleId, PermissionCollection permissions)
        {
            this._roleId = roleId;
            this._permissions = permissions;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The role id to which data security belongs
        /// </summary>
        [DataMember]
        public Int32 RoleId
        {
            get
            {
                return _roleId;
            }
            set
            {
                _roleId = value;
            }
        }

        /// <summary>
        /// The collection of permissions for the requested role id
        /// </summary>
        [DataMember]
        public PermissionCollection Permissions
        {
            get
            {
                return _permissions;
            }
            set
            {
                _permissions = value;
            }
        }
        
        #endregion

        #region Methods

        #region ICloneable Members

        /// <summary>
        /// Clones instance
        /// </summary>
        /// <returns>Cloned object</returns>
        public object Clone()
        {
            return
                new DataSecurity(_roleId, (this._permissions == null) ? null : (PermissionCollection)this._permissions.Clone());
        }

        #endregion

        #endregion
    }
}
