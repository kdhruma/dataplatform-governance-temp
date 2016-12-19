using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class represents an entity model context
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityModelContext : ObjectBase, IEntityModelContext
    {
        #region Fields

        /// <summary>
        /// Specifies the Organization Id for the entity model
        /// </summary>
        [DataMember]
        [ProtoMember(1), DefaultValue(0)]
        private Int32 _organizationId = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2), DefaultValue(0)]
        private Int32 _containerId = 0;

        /// <summary>
        /// Field indicates id of container qualifier
        /// </summary>
        [DataMember]
        [ProtoMember(3), DefaultValue(0)]
        private Int32 _containerQualifierId = 0;

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        [ProtoMember(4), DefaultValue(0)]
        private Int32 _hierarchyId = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(5), DefaultValue(0)]
        private Int32 _entityTypeId;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(6), DefaultValue(0)]
        private Int64 _categoryId;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        private Collection<Int32> _relationshipTypeIds;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        private String _organizationName;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        private String _containerName;

        /// <summary>
        /// Field indicates name of container qualifier
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        private String _containerQualifierName = "None";

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        private String _hierarchyName;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        private String _entityTypeName;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        private String _categoryPath;

        /// <summary>
        /// 
        /// </summary>
        private Collection<String> _relationshipTypeNames;

        #endregion

        #region Constructor

        /// <summary>
        /// Represents the default constructor
        /// </summary>
        public EntityModelContext()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the Organization Id for the entity model
        /// </summary>
        public Int32 OrganizationId
        {
            get
            {
                return _organizationId;
            }
            set
            {
                _organizationId = value;
            }
        }

        /// <summary>
        /// Specifies the container Id for the entity model
        /// </summary>
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
        /// Property denoting id of container qualifier
        /// </summary>
        public Int32 ContainerQualifierId
        {
            get
            {
                return _containerQualifierId;
            }
            set
            {
                _containerQualifierId = value;
            }
        }

        /// <summary>
        /// Property denoting name of container qualifier
        /// </summary>
        public String ContainerQualifierName
        {
            get
            {
                return _containerQualifierName;
            }
            set
            {
                _containerQualifierName = value;
            }
        }

        /// <summary>
        /// Specifies the hierarchy Id for the entity model
        /// </summary>
        public Int32 HierarchyId
        {
            get
            {
                return _hierarchyId;
            }
            set
            {
                _hierarchyId = value;
            }
        }

        /// <summary>
        /// Specifies the entity type Id for the entity model
        /// </summary>
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
        /// Specifies the category Id for the entity model
        /// </summary>
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
        /// Specifies the List of relationship type Id for the entity model
        /// </summary>
        public Collection<Int32> RelationshipTypeIds
        {
            get
            {
                return _relationshipTypeIds;
            }
            set
            {
                _relationshipTypeIds = value;
            }
        }

        /// <summary>
        /// Specifies the organization short name for the entity model
        /// </summary>
        public String OrganizationName
        {
            get
            {
                return _organizationName;
            }
            set
            {
                _organizationName = value;
            }
        }

        /// <summary>
        /// Specifies the container short name for the entity model
        /// </summary>
        public String ContainerName
        {
            get
            {
                return _containerName;
            }
            set
            {
                _containerName = value;
            }
        }

        /// <summary>
        /// Specifies the hierarchy short name for the entity model
        /// </summary>
        public String HierarchyName
        {
            get
            {
                return _hierarchyName;
            }
            set
            {
                _hierarchyName = value;
            }
        }

        /// <summary>
        /// Specifies the entity type short name for the entity model
        /// </summary>
        public String EntityTypeName
        {
            get
            {
                return _entityTypeName;
            }
            set
            {
                _entityTypeName = value;
            }
        }

        /// <summary>
        /// Specifies the category path for the entity model
        /// </summary>
        public String CategoryPath
        {
            get
            {
                return _categoryPath;
            }
            set
            {
                _categoryPath = value;
            }
        }

        /// <summary>
        /// Specifies the list of relationship type short name for the entity model
        /// </summary>
        public Collection<String> RelationshipTypeNames
        {
            get
            {
                return _relationshipTypeNames;
            }
            set
            {
                _relationshipTypeNames = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clones entity model context object
        /// </summary>
        /// <returns></returns>
        public IEntityModelContext Clone()
        {
            var clonedEntityModelContext = new EntityModelContext
            {
                OrganizationId = OrganizationId,
                ContainerId = ContainerId,
                ContainerQualifierId = ContainerQualifierId,
                HierarchyId = HierarchyId,
                EntityTypeId = EntityTypeId,
                CategoryId = CategoryId,
                RelationshipTypeIds =
                RelationshipTypeIds,
                OrganizationName = OrganizationName,
                ContainerName = ContainerName,
                ContainerQualifierName = ContainerQualifierName,
                HierarchyName = HierarchyName,
                EntityTypeName = EntityTypeName,
                CategoryPath = CategoryPath,
                RelationshipTypeNames = RelationshipTypeNames
            };

            return clonedEntityModelContext;
        }

        #endregion
    }
}