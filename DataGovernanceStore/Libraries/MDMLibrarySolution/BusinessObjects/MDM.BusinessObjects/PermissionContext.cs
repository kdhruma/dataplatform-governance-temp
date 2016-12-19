using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the context for permission objects
    /// </summary>
    [DataContract]
    public class PermissionContext : ObjectBase, IPermissionContext, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denoting the role id
        /// </summary>
        private Int32 _roleId = 0;

        /// <summary>
        /// Field denoting the user id
        /// </summary>
        private Int32 _userId = 0;

        /// <summary>
        /// Field denoting the Org Id
        /// </summary>
        private Int32 _orgId = 0;

        /// <summary>
        /// Field denoting the Container Id
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting the Category Id
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting the Entity Type Id
        /// </summary>
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field denoting the Entity Id
        /// </summary>
        private Int64 _entityId = 0;

        /// <summary>
        /// Field denoting the Attribute Id
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        /// Field denoting the Relationship Type Id
        /// </summary>
        private Int32 _relationshipTypeId = 0;

        /// <summary>
        /// Field denoting the Locale Id
        /// </summary>
        private Int32 _localeId = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Permission Context class
        /// </summary>
        public PermissionContext()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Permission Context class
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="containerId"></param>
        /// <param name="categoryId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="relationshipTypeId"></param>
        /// <param name="entityId"></param>
        /// <param name="attributeId"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <param name="localeId"></param>
        public PermissionContext( Int32 orgId, Int32 containerId, Int64 categoryId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 entityId, Int32 attributeId, Int32 roleId, Int32 userId, Int32 localeId )
        {
            _orgId = orgId;
            _containerId = containerId;
            _categoryId = categoryId;
            _entityTypeId = entityTypeId;
            _relationshipTypeId = relationshipTypeId;
            _entityId = entityId;
            _attributeId = attributeId;
            _roleId = roleId;
            _userId = userId;
            _localeId = localeId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the role id
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
        /// Property denoting the user id
        /// </summary>
        [DataMember]
        public Int32 UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        /// <summary>
        /// Property denoting the Org Id
        /// </summary>
        [DataMember]
        public Int32 OrgId
        {
            get
            {
                return _orgId;
            }
            set
            {
                _orgId = value;
            }
        }

        /// <summary>
        /// Property denoting the Container Id
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get
            {
                return _containerId;
            }
            set
            {
                _containerId = value;
            }
        }

        /// <summary>
        /// Property denoting the Category Id
        /// </summary>
        [DataMember]
        public Int64 CategoryId
        {
            get
            {
                return _categoryId;
            }
            set
            {
                _categoryId = value;
            }
        }

        /// <summary>
        /// Property denoting the Entity Type Id
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get
            {
                return _entityTypeId;
            }
            set
            {
                _entityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting the Entity Id
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get
            {
                return _entityId;
            }
            set
            {
                _entityId = value;
            }
        }

        /// <summary>
        /// Property denoting the Attribute Id
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get
            {
                return _attributeId;
            }
            set
            {
                _attributeId = value;
            }
        }

        /// <summary>
        /// Property denoting the Relationship Type Id
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get
            {
                return _relationshipTypeId;
            }
            set
            {
                _relationshipTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting the Locale Id
        /// </summary>
        [DataMember]
        public Int32 LocaleId
        {
            get
            {
                return _localeId;
            }
            set
            {
                _localeId = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, False.</returns>
        public override bool Equals(object obj)
        {
            //TODO: base.Equals(obj) is incorrect here because Equals method is not implemented on ObjectBase class level
            if (base.Equals(obj))
            {
                if (obj is PermissionContext)
                {
                    PermissionContext objectToBeCompared = obj as PermissionContext;

                    if (this.RoleId != objectToBeCompared.RoleId)
                        return false;

                    if (this.UserId != objectToBeCompared.UserId)
                        return false;

                    if (this.OrgId != objectToBeCompared.OrgId)
                        return false;

                    if (this.ContainerId != objectToBeCompared.ContainerId)
                        return false;

                    if (this.CategoryId != objectToBeCompared.CategoryId)
                        return false;

                    if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                        return false;

                    if (this.EntityId != objectToBeCompared.EntityId)
                        return false;

                    if (this.AttributeId != objectToBeCompared.AttributeId)
                        return false;

                    if (this.RelationshipTypeId != objectToBeCompared.RelationshipTypeId)
                        return false;

                    if (this.LocaleId != objectToBeCompared.LocaleId)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            
            //TODO: base.GetHashCode() is incorrect here because GetHashCode method is not implemented on ObjectBase class level
            hashCode = base.GetHashCode() ^ this.AttributeId.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.ContainerId.GetHashCode() ^ 
                this.EntityId.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.LocaleId.GetHashCode() ^ this.ObjectType.GetHashCode() ^ 
                this.ObjectTypeId.GetHashCode() ^ this.OrgId.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^ this.RoleId.GetHashCode() ^ 
                this.UserId.GetHashCode();

            return hashCode;
        }

        #region ICloneable Members

        /// <summary>
        /// Clones instance
        /// </summary>
        /// <returns>Cloned object</returns>
        public object Clone()
        {
            return
                new PermissionContext(this._orgId, this._containerId, this._categoryId, this._entityTypeId,
                    this._relationshipTypeId, this._entityId, this._attributeId, this._roleId, this._userId,
                    this._localeId);
        }

        #endregion

        #endregion
    }
}
