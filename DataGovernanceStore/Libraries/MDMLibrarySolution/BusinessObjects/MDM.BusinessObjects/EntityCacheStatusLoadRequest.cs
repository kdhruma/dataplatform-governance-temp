using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the request object for loading entity cache status.
    /// </summary>
    public class EntityCacheStatusLoadRequest : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Holds the entity id for which the cache status should be requested.
        /// </summary>
        private Int64 _entityId;

        /// <summary>
        /// Holds the container id for the entity for which the cache status should be requested.
        /// </summary>
        private Int32 _containerId;

        /// <summary>
        /// Holds the parent tree id list for the entity for which the cache status should be requested.
        /// </summary>
        private String _parentEntityTreeIdList;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor for EntityCacheStatusLoadRequest
        /// </summary>
        public EntityCacheStatusLoadRequest() : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the entity id for which the cache status should be requested.
        /// </summary>
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Indicates the container id for the entity for which the cache status should be requested.
        /// </summary>
        public Int32 ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Indicates the parent tree id list for the entity for which the cache status should be requested.
        /// </summary>
        public String ParentEntityTreeIdList
        {
            get { return _parentEntityTreeIdList; }
            set { _parentEntityTreeIdList = value; }
        }

        #endregion
    }
}
