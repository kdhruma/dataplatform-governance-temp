using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// A context representing what all data to be written in ToXML and LoadXML 
    /// </summary>
    [DataContract]
    public class EntityConversionContext : ObjectBase
    {
        #region fields

        /// <summary>
        /// Indicates whether to load RelatedEntity into HierarchyRelationship
        /// </summary>
        private Boolean _loadRelatedEntityInHierarchyRelationship = false;

        #endregion fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityConversionContext()
        {

        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// A property denoting whether to load RelatedEntity into HierarchyRelationship
        /// </summary>
        [DataMember]
        public Boolean LoadRelatedEntityInHierarchyRelationship
        {
            get { return _loadRelatedEntityInHierarchyRelationship; }
            set { _loadRelatedEntityInHierarchyRelationship = value; }
        }

        #endregion Properties
    }
}
