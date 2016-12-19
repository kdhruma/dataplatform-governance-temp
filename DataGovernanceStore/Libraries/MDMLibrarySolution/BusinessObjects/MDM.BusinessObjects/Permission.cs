using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the type of the permission
    /// </summary>
    [DataContract]
    public enum PermissionType
    {
        /// <summary>
        /// Implicit permission
        /// </summary>
        [EnumMember]
        Implicit = 1,

        /// <summary>
        /// Explicit permission
        /// </summary>
        [EnumMember]
        Explicit = 2
    }

    /// <summary>
    /// Specifies the Permissions for the MDM Object
    /// </summary>
    [DataContract]
    [KnownType(typeof(UserAction))]
    [KnownType(typeof(PermissionType))]
    [KnownType(typeof(PermissionContext))]
    public class Permission : ObjectBase,IPermission,ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denoting the id of the object
        /// </summary>
        private Int64 _objectId = 0;

        /// <summary>
        /// Field denoting the set of permissions
        /// </summary>
        private Collection<UserAction> _permissionSet = new Collection<UserAction>();

        /// <summary>
        /// Field denoting the type of the permission
        /// </summary>
        private PermissionType _permissionType = PermissionType.Implicit;

        /// <summary>
        /// Field denoting the parent permission object
        /// Requires when we have derived / implicit permission...
        /// </summary>
        private Permission _parent = null;

        /// <summary>
        /// Field denoting the context of the permission object
        /// </summary>
        private PermissionContext _context = new PermissionContext();

        /// <summary>
        /// Field denoting the context sequence
        /// </summary>
        private Int32 _sequence = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Permission class.
        /// </summary>
        public Permission()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Permission class.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="objectTypeId"></param>
        /// <param name="objectType"></param>
        /// <param name="permissionSet"></param>
        /// <param name="context"></param>
        /// <param name="sequence"></param>
        public Permission(Int64 objectId, Int32 objectTypeId, String objectType, Collection<UserAction> permissionSet, PermissionContext context, Int32 sequence)
            : base(objectTypeId, objectType)
        {
            _objectId = objectId;
            _permissionSet = permissionSet;
            _context = context;
            _sequence = sequence;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the id of the object
        /// </summary>
        [DataMember]
        public Int64 ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                _objectId = value;
            }
        }

        /// <summary>
        /// Property denoting the set of permissions
        /// </summary>
        [DataMember]
        public Collection<UserAction> PermissionSet
        {
            get
            {
                return _permissionSet;
            }
            set
            {
                _permissionSet = value;
            }
        }

        /// <summary>
        /// Property denoting the type of the permission
        /// </summary>
        [DataMember]
        public PermissionType PermissionType
        {
            get
            {
                return _permissionType;
            }
            set
            {
                _permissionType = value;
            }
        }

        /// <summary>
        /// Property denoting the parent permission object
        /// Requires when we have derived / implicit permission...
        /// </summary>
        [DataMember]
        public Permission Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// Property denoting the context of the permission object
        /// </summary>
        [DataMember]
        public PermissionContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        /// <summary>
        /// Property denoting the context sequence
        /// </summary>
        [DataMember]
        public Int32 Sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets  permission context
        /// </summary>
        /// <returns> Permission context interface</returns>
        public  IPermissionContext GetPermissionContext()
        {
            return this.Context;
        }

        /// <summary>
        /// Sets permission context
        /// </summary>
        /// <param name="iPermissionContext"> Permission context interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed  permission context is null</exception>
        public void SetPermissionContext(IPermissionContext iPermissionContext)
        {
            if (iPermissionContext == null)
                throw new ArgumentNullException("PermissionContext");

            this.Context = (PermissionContext)iPermissionContext;
        }

        /// <summary>
        /// Gets parent permission
        /// </summary>
        /// <returns> Parent permission interface</returns>
        public IPermission GetParentPermission()
        {
            return this.Parent;
        }

        /// <summary>
        /// Sets parent permission
        /// </summary>
        /// <param name="iPermission"> Parent Permission interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed  permission is null</exception>
        public void SetParentPermission(IPermission iPermission)
        {
            if (iPermission == null)
                throw new ArgumentNullException("Parent Permission");

            this.Parent = (Permission)iPermission;
        }

        #region ICloneable Members

        /// <summary>
        /// Clones instance
        /// </summary>
        /// <returns>Cloned object</returns>
        public object Clone()
        {
            Collection<UserAction> clonedPermissionSet = null;
            if (this._permissionSet != null)
            {
                clonedPermissionSet = new Collection<UserAction>();
                for (Int32 i = 0; i < this._permissionSet.Count; i++)
                {
                    clonedPermissionSet.Add(this._permissionSet[i]);
                }
            }
            return
                new Permission(this._objectId, this.ObjectTypeId, this.ObjectType, clonedPermissionSet,
                    (this._context == null) ? null : ((PermissionContext) this._context.Clone()), this._sequence);
        }

        #endregion

        #endregion

        #endregion
    }
}
