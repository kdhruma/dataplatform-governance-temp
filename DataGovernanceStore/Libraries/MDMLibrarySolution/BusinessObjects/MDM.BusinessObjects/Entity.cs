using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using Core;
    using MDM.Interfaces;
    using Workflow;

    /// <summary>
    /// Specifies the Entity
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(MDMObject))]
    [KnownType(typeof(Attribute))]
    [KnownType(typeof(ObjectAction))]
    [KnownType(typeof(Relationship))]
    [KnownType(typeof(RelationshipCollection))]
    [KnownType(typeof(EntityContext))]
    [KnownType(typeof(AttributeCollection))]
    [KnownType(typeof(HierarchyRelationship))]
    [KnownType(typeof(HierarchyRelationshipCollection))]
    [KnownType(typeof(ExtensionRelationship))]
    [KnownType(typeof(ExtensionRelationshipCollection))]
    [KnownType(typeof(AttributeModelCollection))]
    [KnownType(typeof(WorkflowStateCollection))]
    [KnownType(typeof(BusinessConditionStatus))]
    [KnownType(typeof(BusinessConditionStatusCollection))]
    public class Entity : MDMObject, IEntity
    {
        #region Fields

        /// <summary>
        /// Field for the id of an Entity
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field for the Entity Guid
        /// </summary>
        private Guid? _entityGuid = null;

        /// <summary>
        /// Field denoting a reference id for an entity object
        /// </summary>
        private Int64 _referenceId = -1;

        /// <summary>
        /// Field denoting external id for an entity object
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field denoting extension unique Id which groups all the extensions
        /// </summary>
        private Int64 _extensionUniqueId = 0;

        /// <summary>
        /// Field denoting external id of the parent for an entity object
        /// </summary>
        private String _parentExternalId = String.Empty;

        /// <summary>
        /// Field denoting SKU information of an entity
        /// </summary>
        private String _sku = String.Empty;

        /// <summary>
        /// Field denoting the parent Id of an entity
        /// </summary>
        private Int64 _parentEntityId = 0;

        /// <summary>
        /// Field denoting the parent entity type Id of an entity
        /// </summary>
        private Int32 _parentEntityTypeId = 0;

        /// <summary>
        /// Field denoting the parent short name of an entity
        /// </summary>
        private String _parentEntityName = String.Empty;

        /// <summary>
        /// Field denoting the parent long name of an entity
        /// </summary>
        private String _parentEntityLongName = String.Empty;

        /// <summary>
        /// Field denoting the parent extension Id of an entity
        /// </summary>
        private Int64 _parentExtensionEntityId = 0;

        /// <summary>
        /// Field denoting the parent extension short name of an entity
        /// </summary>
        private String _parentExtensionEntityName = String.Empty;

        /// <summary>
        /// Field denoting the parent extension long name of an entity
        /// </summary>
        private String _parentExtensionEntityLongName = String.Empty;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private String _parentExtensionEntityExternalId = String.Empty;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private Int32 _parentExtensionEntityContainerId = 0;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private String _parentExtensionEntityContainerName = String.Empty;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private String _parentExtensionEntityContainerLongName = String.Empty;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private Int64 _parentExtensionEntityCategoryId = 0;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private String _parentExtensionEntityCategoryName = String.Empty;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private String _parentExtensionEntityCategoryLongName = String.Empty;

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        private String _parentExtensionEntityCategoryPath = String.Empty;

        /// <summary>
        /// Property denoting the parent extension category long name path of an entity
        /// </summary>
        private String _parentExtensionEntityCategoryLongNamePath = String.Empty;

        /// <summary>
        /// Field denoting the category ID of an entity
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting the category name of an entity
        /// </summary>
        private String _categoryName = String.Empty;

        /// <summary>
        /// Field denoting the category long name of an entity
        /// </summary>
        private String _categoryLongName = String.Empty;

        /// <summary>
        /// Field denoting the container of an entity
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting the container of an entity
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field denoting the container long name of an entity
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field denoting the type id of an entity
        /// </summary>
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field denoting the type short name of an entity
        /// </summary>
        private String _entityTypeName = string.Empty;

        /// <summary>
        /// Field denoting the type name of an entity
        /// </summary>
        private String _entityTypeLongName = string.Empty;

        /// <summary>
        /// Field denoting organization Id of an entity
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Field denoting organization name of an entity
        /// </summary>
        private String _organizationName = String.Empty;

        /// <summary>
        /// Field denoting organization long name of an entity
        /// </summary>
        private String _organizationLongName = String.Empty;

        /// <summary>
        /// Field denoting ShowMaster of an entity
        /// </summary>
        private Boolean _showMaster = false;

        /// <summary>
        /// Field denoting the path of an entity
        /// </summary>
        private String _path = string.Empty;

        /// <summary>
        /// Field denoting the category path of an entity
        /// </summary>
        private String _categoryPath = string.Empty;

        /// <summary>
        /// Field denoting the category long name path of an entity
        /// </summary>
        private String _categoryLongNamePath = string.Empty;

        /// <summary>
        /// Field denoting the id Path of an entity
        /// </summary>
        private String _idPath = string.Empty;

        /// <summary>
        /// Field denoting the branch level of an entity
        /// </summary>
        private ContainerBranchLevel _branchLevel = ContainerBranchLevel.Component;

        /// <summary>
        /// Field denoting the list of attributes of an entity. It can have both common and technical attributes
        /// </summary>
        [DataMember]
        [ProtoMember(41)]
        private AttributeCollection _attributes = null;

        /// <summary>
        /// Field denoting the list of relationships of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(42)]
        private RelationshipCollection _relationships = null;

        /// <summary>
        /// Field denoting the list of hierarchy relationships of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(43)]
        private HierarchyRelationshipCollection _hierarchyRelationships = null;

        /// <summary>
        /// Field denoting the list of extension relationships of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(44)]
        private ExtensionRelationshipCollection _extensionRelationships = null;

        /// <summary>
        /// Field denoting the sequence number assigned to entity while participating in queue management system(Ex: Export Queue, Parellel Execution Queue, etc)
        /// </summary>
        private Int32 _sequenceNumber = -1;

        /// <summary>
        /// Field showing which all information is loaded for current entity
        /// </summary>
        [DataMember]
        [ProtoMember(46)]
        private EntityContext _entityContext = null;

        /// <summary>
        /// Presents all the changes made in the this entity
        /// </summary>
        private EntityChangeContext _entityChangeContext = null;

        /// <summary>
        /// Field denoting change context for entity post process
        /// </summary>
        private EntityChangeContext _entityPostChangeContext = null;

        /// <summary>
        /// Field denoting related entity's(Parent or Child) changes impacting the current entity
        /// </summary>
        private EntityChangeContext _relatedEntityChangeContext = null;

        /// <summary>
        /// Used for Reclassification
        /// </summary>
        [DataMember]
        [ProtoMember(47)]
        private EntityMoveContext _entityMoveContext = null;

        /// <summary>
        /// Field denoting permission set for the current entity.
        /// </summary>
        [DataMember]
        [ProtoMember(48)]
        private Collection<UserAction> _permissionSet = null;

        /// <summary>
        /// Field Denoting AttributeModel Collection
        /// </summary>
        [DataMember]
        [ProtoMember(49)]
        private AttributeModelCollection _attributeModels = null;

        /// <summary>
        /// Indicates relationship attribute models per relationship type
        /// </summary>
        private Dictionary<Int32,AttributeModelCollection> _relationshipsAttributeModels = null;

        /// <summary>
        /// Field Denoting validation state 
        /// </summary>
        private ValidationStates _validationStates = null;

        /// <summary>
        /// Indicates State validations
        /// </summary>
        private EntityStateValidationCollection _entityStateValidations = null;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Int32, Dictionary<String, String>> _overridenAttributeModelProperties = null;

        /// <summary>
        /// Indicates source class for current entity.
        /// </summary>
        private Int32 _sourceClass = 0;

        /// <summary>
        /// Field Denoting workflow state of current entity 
        /// </summary>
        private WorkflowStateCollection _workflowStates = null;

        /// <summary>
        /// Represents ActionContext for workflow. Hold current activity being performed on current entity.
        /// </summary>
        private WorkflowActionContext _workflowActionContext = null;

        /// <summary>
        /// Field Denoting the Original Entity 
        /// </summary>
        private Entity _originalEntity = null;

        /// <summary>
        /// Indicates distinct Ids of current entity's parents. This will include entity Ids of all parents (straight parent and MDL parent).
        /// </summary>
        private String _entityTreeIdList = String.Empty;

        /// <summary>
        /// Field denoting whether the current entity is having direct changes or having changes impacted from related entity
        /// </summary>
        private Boolean _isDirectChange = true;

        /// <summary>
        /// ActivtyLogId- It can be its own or from the parent
        /// </summary>
        private Int64 _activityLogId = 0;

        /// <summary>
        /// Property defines which program is the source info of changes of object
        /// </summary>
        private SourceInfo _sourceInfo;

        /// <summary>
        /// Field denoting entity family id
        /// </summary>
        private Int64 _entityFamilyId = 0;

        /// <summary>
        /// Field denoting entity family group id
        /// </summary>
        private Int64 _entityGlobalFamilyId = 0;

        /// <summary>
        /// Field denoting entity hierarchy level
        /// </summary>
        private Int16 _hierarchyLevel = 0;

        /// <summary>
        /// Field denoting entity cross reference id. Indicates the Approved entity id in case of Collaboration entity and represents the Collaboration entity id in case of Approved entity.
        /// </summary>
        private Int64 _crossReferenceId = -1;

        /// <summary>
        /// Field denoting entity business condition status collection
        /// </summary>
        private BusinessConditionStatusCollection _businessConditions = null;

        /// <summary>
        /// Indicates entity family name for a variant tree
        /// </summary>
        private String _entityFamilyLongName = String.Empty;

        /// <summary>
        /// Indicates entity global family name across parent(including extended families)
        /// </summary>
        private String _entityGlobalFamilyLongName = String.Empty;
        
        /// <summary>
        /// Field denoting the cross reference guid
        /// </summary>
        private Guid? _crossReferenceGuid = null;

        /// <summary>
        /// Indicates list of rule map context ids which are associated with an entity
        /// </summary>
        private Collection<Int32> _ruleMapContextIdList = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Entity()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Entity</param>
        public Entity(Int64 id)
            : base()
        {
            this._id = id;
        }

        /// <summary>
        /// Constructor with Id, Name and Description of an Entity as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Entity</param>
        /// <param name="name">Indicates the Short Name of an Entity</param>
        /// <param name="longName">Indicates the Long name of an Entity</param>
        public Entity(Int64 id, String name, String longName)
            : base()
        {
            this._id = id;
            this.Name = name;
            this.LongName = longName;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an Entity as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Entity</param>
        /// <param name="name">Indicates the Name of an Entity</param>
        /// <param name="longName">Indicates the LongName of an Entity</param>
        /// <param name="locale">Indicates the Locale of an Entity</param>
        public Entity(Int64 id, String name, String longName, LocaleEnum locale)
            : base()
        {
            this._id = id;
            this.Name = name;
            this.LongName = longName;
            this.Locale = locale;
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Entity Id="1435733" 
        ///             ObjectId="200" 
        ///             Name="J7470" 
        ///             LongName="J7470" 
        ///             SKU="J7470" 
        ///             Path="Fingerhut#@#Merchandising Catalog#@#DM#@#051#@#334#@#J7470" 
        ///             CategoryPath="DM#@#051"
        ///             BranchLevel="2" 
        ///             ParentEntityId="293" 
        ///             ParentEntityName="334" 
        ///             ParentEntityLongName="Quilts/Puffs" 
        ///             CategoryId="293" 
        ///             CategoryName="334" 
        ///             CategoryLongName="Quilts/Puffs" 
        ///             ContainerId="17" 
        ///             ContainerName="Merchandising Catalog" 
        ///             ContainerLongName="Merchandising Catalog" 
        ///             OrganizationId="101" 
        ///             OrganizationName="Fingerhut" 
        ///             OrganizationLongName="Fingerhut Corporation" 
        ///             EntityTypeId="7" 
        ///             EntityTypeName="4" 
        ///             EntityTypeLongName="Product" 
        ///             EffectiveFrom="2007-05-03T00:00:00" 
        ///             EffectiveTo="2109-04-06T01:41:35.357" 
        ///             EntityLastModTime="2009-04-06T01:42:00" 
        ///             EntityLastModUser="RIVERSAND\ramanim" /&gt;
        /// </para>
        /// </example>
        public Entity(String valuesAsXml)
        {
            LoadEntityDetail(valuesAsXml, ObjectSerialization.Full);
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having XML values</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public Entity(String valuesAsXml, ObjectSerialization objectSerialization)
        {
            LoadEntityDetail(valuesAsXml, objectSerialization);
        }

        /// <summary>
        /// Constructor which takes id, objectId, name, longName, sku, path, parentId, parentName, CatalogId, CatalogName, orgId, entityTypeId, entityTypeName as input 
        /// </summary>
        /// <param name="id">Indicates the identity of an entity</param>
        /// <param name="extensionUniqueId">Indicates the extension unique Id</param>
        /// <param name="name">Indicates the short name of an entity</param>
        /// <param name="longName">Indicates the long name of an entity</param>
        /// <param name="sku">Indicates the SKU information of an entity </param>
        /// <param name="path">Indicates the path of an entity</param>
        /// <param name="parentId">Indicates the identity of parent of an entity</param>
        /// <param name="parentLongName">Indicates the long name of parent of an entity</param>
        /// <param name="parentExtensionId">Indicates the identity of extension parent of an entity</param>
        /// <param name="parentExtensionLongName">Indicates the long name of extension parent of an entity</param>
        /// <param name="catalogId">Indicates the identity of catalog of an entity</param>
        /// <param name="catalogLongName">Indicates the catalog long name of an entity</param>
        /// <param name="orgId">Indicates the identity of organization of an entity</param>
        /// <param name="entityTypeId">Indicates the identity of entityType</param>
        /// <param name="entityTypeName">Indicates the name of entityType</param>
        public Entity(Int64 id, Int64 extensionUniqueId, String name, String longName, String sku, String path, Int64 parentId, String parentLongName, Int64 parentExtensionId, String parentExtensionLongName, Int32 catalogId, String catalogLongName,
            Int32 orgId, Int32 entityTypeId, String entityTypeName)
            : base()
        {
            this._id = id;
            this.Name = name;
            this.LongName = longName;
            this._extensionUniqueId = extensionUniqueId;
            this._sku = sku;
            this._path = path;
            this._parentEntityId = parentId;
            this._parentEntityLongName = parentLongName;
            this._parentExtensionEntityId = parentExtensionId;
            this._parentExtensionEntityLongName = parentExtensionLongName;
            this._containerId = catalogId;
            this._containerLongName = catalogLongName;
            this._organizationId = orgId;
            this._entityTypeId = entityTypeId;
            this._entityTypeName = entityTypeName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the id of an Entity
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "Entity";
            }
        }

        /// <summary>
        /// Property denoting extension unique Id which groups all the extensions
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 ExtensionUniqueId
        {
            get
            {
                return _extensionUniqueId;
            }
            set
            {
                _extensionUniqueId = value;
            }
        }

        /// <summary>
        /// Property denoting the external id for an entity object
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public new Int64 ReferenceId
        {
            get
            {
                return this._referenceId;
            }
            set
            {
                this._referenceId = value;
            }
        }

        /// <summary>
        /// Property denoting the external id for an entity object
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String ExternalId
        {
            get
            {
                return this._externalId;
            }
            set
            {
                this._externalId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent external id of this entity object
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String ParentExternalId
        {
            get
            {
                return this._parentExternalId;
            }
            set
            {
                this._parentExternalId = value;
            }
        }

        /// <summary>
        /// Property denoting SKU information of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String SKU
        {
            get { return _sku; }
            set { _sku = value; }
        }

        /// <summary>
        /// Property denoting the parent Id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Int64 ParentEntityId
        {
            get
            {
                return this._parentEntityId;
            }
            set
            {
                this._parentEntityId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent entity type id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public Int32 ParentEntityTypeId
        {
            get
            {
                return this._parentEntityTypeId;
            }
            set
            {
                this._parentEntityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent short name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public String ParentEntityName
        {
            get
            {
                return this._parentEntityName;
            }
            set
            {
                this._parentEntityName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public String ParentEntityLongName
        {
            get
            {
                return this._parentEntityLongName;
            }
            set
            {
                this._parentEntityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension Id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public Int64 ParentExtensionEntityId
        {
            get
            {
                return this._parentExtensionEntityId;
            }
            set
            {
                this._parentExtensionEntityId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public String ParentExtensionEntityName
        {
            get
            {
                return this._parentExtensionEntityName;
            }
            set
            {
                this._parentExtensionEntityName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public String ParentExtensionEntityLongName
        {
            get
            {
                return this._parentExtensionEntityLongName;
            }
            set
            {
                this._parentExtensionEntityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public String ParentExtensionEntityExternalId
        {
            get
            {
                return this._parentExtensionEntityExternalId;
            }
            set
            {
                this._parentExtensionEntityExternalId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public Int32 ParentExtensionEntityContainerId
        {
            get
            {
                return this._parentExtensionEntityContainerId;
            }
            set
            {
                this._parentExtensionEntityContainerId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        public String ParentExtensionEntityContainerName
        {
            get
            {
                return this._parentExtensionEntityContainerName;
            }
            set
            {
                this._parentExtensionEntityContainerName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
        public String ParentExtensionEntityContainerLongName
        {
            get
            {
                return this._parentExtensionEntityContainerLongName;
            }
            set
            {
                this._parentExtensionEntityContainerLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        public Int64 ParentExtensionEntityCategoryId
        {
            get
            {
                return this._parentExtensionEntityCategoryId;
            }
            set
            {
                this._parentExtensionEntityCategoryId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(19)]
        public String ParentExtensionEntityCategoryName
        {
            get
            {
                return this._parentExtensionEntityCategoryName;
            }
            set
            {
                this._parentExtensionEntityCategoryName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(20)]
        public String ParentExtensionEntityCategoryLongName
        {
            get
            {
                return this._parentExtensionEntityCategoryLongName;
            }
            set
            {
                this._parentExtensionEntityCategoryLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension external id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(21)]
        public String ParentExtensionEntityCategoryPath
        {
            get
            {
                return this._parentExtensionEntityCategoryPath;
            }
            set
            {
                this._parentExtensionEntityCategoryPath = value;
            }
        }

        /// <summary>
        /// Property denoting the parent extension category long name path of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(22)]
        public String ParentExtensionEntityCategoryLongNamePath
        {
            get { return _parentExtensionEntityCategoryLongNamePath; }
            set { _parentExtensionEntityCategoryLongNamePath = value; }
        }

        /// <summary>
        /// Property denoting the category id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(23)]
        public Int64 CategoryId
        {
            get
            {
                return this._categoryId;
            }
            set
            {
                this._categoryId = value;
            }
        }

        /// <summary>
        /// Property denoting the category name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(24)]
        public String CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        /// <summary>
        /// Property denoting the container long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(25)]
        public String CategoryLongName
        {
            get { return _categoryLongName; }
            set { _categoryLongName = value; }
        }

        /// <summary>
        /// Property denoting the container of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(26)]
        public Int32 ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                this._containerId = value;
            }
        }

        /// <summary>
        /// Property denoting the container of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(27)]
        public String ContainerName
        {
            get
            {
                return this._containerName;
            }
            set
            {
                this._containerName = value;
            }
        }

        /// <summary>
        /// Property denoting the container long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(28)]
        public string ContainerLongName
        {
            get
            {
                return this._containerLongName;
            }
            set
            {
                this._containerLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the type id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(29)]
        public Int32 EntityTypeId
        {
            get
            {
                return this._entityTypeId;
            }
            set
            {
                this._entityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting the type short name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(30)]
        public String EntityTypeName
        {
            get { return _entityTypeName; }
            set { _entityTypeName = value; }
        }

        /// <summary>
        /// Property denoting the type name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(31)]
        public String EntityTypeLongName
        {
            get { return _entityTypeLongName; }
            set { _entityTypeLongName = value; }
        }

        /// <summary>
        /// Indicates the Organization id of an Entity
        /// </summary>
        [DataMember]
        [ProtoMember(32)]
        public Int32 OrganizationId
        {
            get
            {
                return this._organizationId;
            }
            set
            {
                this._organizationId = value;
            }
        }

        /// <summary>
        /// Property denoting organization name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(33)]
        public String OrganizationName
        {
            get
            {
                return this._organizationName;
            }
            set
            {
                this._organizationName = value;
            }
        }

        /// <summary>
        /// Property denoting organization long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(34)]
        public String OrganizationLongName
        {
            get
            {
                return this._organizationLongName;
            }
            set
            {
                this._organizationLongName = value;
            }
        }

        /// <summary>
        /// Property denoting ShowMaster of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(35)]
        public Boolean ShowMaster
        {
            get
            {
                return this._showMaster;
            }
            set
            {
                this._showMaster = value;
            }
        }

        /// <summary>
        /// Property denoting the path of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(36)]
        public String Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }

        /// <summary>
        /// Property denoting the Category path of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(37)]
        public String CategoryPath
        {
            get
            {
                return this._categoryPath;
            }
            set
            {
                this._categoryPath = value;
            }
        }

        /// <summary>
        /// Property denoting the Category long name path of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(38)]
        public String CategoryLongNamePath
        {
            get
            {
                return this._categoryLongNamePath;
            }
            set
            {
                this._categoryLongNamePath = value;
            }
        }
        /// <summary>
        /// Property denoting the IDPath of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(39)]
        public String IdPath
        {
            get
            {
                return this._idPath;
            }
            set
            {
                this._idPath = value;
            }
        }

        /// <summary>
        /// Property denoting the branch level of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(40)]
        public ContainerBranchLevel BranchLevel
        {
            get
            {
                return this._branchLevel;
            }
            set
            {
                this._branchLevel = value;
            }
        }

        /// <summary>
        /// Property denoting the list of attributes of an entity. It can have both common and technical attributes
        /// </summary>
        public AttributeCollection Attributes
        {
            get
            {
                if (this._attributes == null)
                {
                    this._attributes = new AttributeCollection();
                }

                return this._attributes;
            }
            set
            {
                this._attributes = value;
            }
        }

        /// <summary>
        /// Property denoting the list of relationships of an entity
        /// </summary>
        public RelationshipCollection Relationships
        {
            get
            {
                if (this._relationships == null)
                {
                    this._relationships = new RelationshipCollection();
                }

                return this._relationships;
            }
            set
            {
                this._relationships = value;
            }
        }

        /// <summary>
        /// Property denoting the list of hierarchy relationships of an entity
        /// </summary>
        public HierarchyRelationshipCollection HierarchyRelationships
        {
            get
            {
                if (this._hierarchyRelationships == null)
                {
                    this._hierarchyRelationships = new HierarchyRelationshipCollection();
                }

                return _hierarchyRelationships;
            }
            set
            {
                _hierarchyRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting the list of extension relationships of an entity
        /// </summary>
        public ExtensionRelationshipCollection ExtensionRelationships
        {
            get
            {
                if (this._extensionRelationships == null)
                {
                    this._extensionRelationships = new ExtensionRelationshipCollection();
                }

                return _extensionRelationships;
            }
            set
            {
                _extensionRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting the sequence number assigned to entity while participating in queue management system(Ex: Export Queue, Parellel Execution Queue, etc)
        /// </summary>
        [DataMember]
        [ProtoMember(45)]
        public Int32 SequenceNumber
        {
            get { return _sequenceNumber; }
            set { _sequenceNumber = value; }
        }

        /// <summary>
        /// Property showing which all information is loaded for current entity
        /// </summary>
        public EntityContext EntityContext
        {
            get
            {
                if (this._entityContext == null)
                {
                    this._entityContext = new EntityContext();
                }

                return this._entityContext;
            }
            set
            {
                this._entityContext = value;
            }
        }

        /// <summary>
        /// Property showing which all information is loaded for current entity		
        /// </summary>
        public EntityMoveContext EntityMoveContext
        {
            get
            {
                if (this._entityMoveContext == null)
                {
                    this._entityMoveContext = new EntityMoveContext();
                }

                return this._entityMoveContext;
            }
            set
            {
                this._entityMoveContext = value;
            }
        }

        /// <summary>
        /// Property denoting related entity's(Parent or Child) changes impacting the current entity	
        /// </summary>
        public EntityChangeContext RelatedEntityChangeContext
        {
            get
            {
                return this._relatedEntityChangeContext;
            }
            set
            {
                this._relatedEntityChangeContext = value;
            }
        }

        /// <summary>
        /// Property denoting change context for entity post process
        /// </summary>
        public EntityChangeContext EntityPostChangeContext
        {
            get
            {
                return this._entityPostChangeContext;
            }
            set
            {
                this._entityPostChangeContext = value;
            }
        }

        /// <summary>
        /// Property showing permission set for the current entity
        /// </summary>
        public new Collection<UserAction> PermissionSet
        {
            get
            {
                if (this._permissionSet == null)
                {
                    this._permissionSet = new Collection<UserAction>();
                }

                return this._permissionSet;
            }
            set
            {
                this._permissionSet = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of AttributeModel
        /// </summary>
        public AttributeModelCollection AttributeModels
        {
            get
            {
                if (this._attributeModels == null)
                {
                    this._attributeModels = new AttributeModelCollection();
                }

                return _attributeModels;
            }
            set
            {
                this._attributeModels = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of AttributeModel
        /// </summary>
        [DataMember]
        [ProtoMember(50)]
        public Dictionary<Int32, AttributeModelCollection> RelationshipsAttributeModels
        {
            get
            {
                if (this._relationshipsAttributeModels == null)
                {
                    this._relationshipsAttributeModels = new Dictionary<Int32, AttributeModelCollection>();
                }

                return _relationshipsAttributeModels;
            }
            set
            {
                this._relationshipsAttributeModels = value;
            }
        }

        /// <summary>
        /// Property denoting Collection of AttributeModel
        /// </summary>
        [DataMember]
        [ProtoMember(51)]
        public Dictionary<Int32, Dictionary<String, String>> OverridenAttributeModelProperties
        {
            get
            {
                return _overridenAttributeModelProperties;
            }
            set
            {
                this._overridenAttributeModelProperties = value;
            }
        }

        /// <summary>
        /// Indicates source class for current entity.
        /// </summary>
        [DataMember]
        [ProtoMember(52)]
        public Int32 SourceClass
        {
            get
            {
                return _sourceClass;
            }
            set
            {
                _sourceClass = value;
            }
        }

        /// <summary>
        /// Indicates Collection of workflow state Information's for the Entity
        /// </summary>
        [DataMember]
        [ProtoMember(53)]
        public WorkflowStateCollection WorkflowStates
        {
            get
            {
                if (this._workflowStates == null)
                {
                    this._workflowStates = new WorkflowStateCollection();
                }

                return _workflowStates;
            }
            set
            {
                _workflowStates = value;
            }
        }

        /// <summary>
        /// Represents ActionContext for workflow. Hold current activity being performed on current entity.
        /// </summary>
        [DataMember]
        [ProtoMember(54)]
        public WorkflowActionContext WorkflowActionContext
        {
            get
            {
                if (this._workflowActionContext == null)
                {
                    this._workflowActionContext = new WorkflowActionContext();
                }

                return _workflowActionContext;
            }

            set
            {
                _workflowActionContext = value;
            }
        }

        /// <summary>
        /// Property denoting the Original Entity
        /// </summary>
        public Entity OriginalEntity
        {
            get
            {
                return _originalEntity;
            }
            set
            {
                this._originalEntity = value;
            }
        }

        /// <summary>
        /// Indicates distinct Ids of current entity's parents. This will include entity Ids of all parents (straight parent and MDL parent).
        /// </summary>
        [DataMember]
        [ProtoMember(55)]
        public String EntityTreeIdList
        {
            get { return _entityTreeIdList; }
            set { _entityTreeIdList = value; }
        }

        /// <summary>
        /// ActivtyLogId- It can be its own or from the parent
        /// </summary>
        public Int64 ActivityLogId
        {
            get { return _activityLogId; }
            set { _activityLogId = value; }
        }

        /// <summary>
        /// Property denoting whether the current entity is having direct changes or having changes impacted from related entity
        /// </summary>
        [DataMember]
        [ProtoMember(56)]
        public Boolean IsDirectChange
        {
            get { return _isDirectChange; }
            set { _isDirectChange = value; }
        }

        /// <summary>
        /// Property defines which program is the source info of changes of object
        /// </summary>
        [DataMember]
        [ProtoMember(57)]
        public SourceInfo SourceInfo
        {
            get
            {
                return _sourceInfo;
            }
            set
            {
                _sourceInfo = value;
            }
        }

        /// <summary>
        /// Specifies entity family id for a variant tree
        /// </summary>
        [DataMember]
        [ProtoMember(58)]
        public Int64 EntityFamilyId
        {
            get { return this._entityFamilyId; }
            set { this._entityFamilyId = value; }
        }

        /// <summary>
        /// Specifies entity global family id across parent(including extended families)
        /// </summary>
        [DataMember]
        [ProtoMember(59)]
        public Int64 EntityGlobalFamilyId
        {
            get { return this._entityGlobalFamilyId; }
            set { this._entityGlobalFamilyId = value; }
        }

        /// <summary>
        /// Property defines entity hierarchy level
        /// </summary>
        [DataMember]
        [ProtoMember(60)]
        public Int16 HierarchyLevel
        {
            get { return _hierarchyLevel; }
            set { _hierarchyLevel = value; }
        }

        /// <summary>
        /// Property defines validation state
        /// </summary>
        [DataMember]
        [ProtoMember(61)]
        public ValidationStates ValidationStates
        {
            get
            {
                if (this._validationStates == null)
                {
                    this._validationStates = new ValidationStates();
                }

                return this._validationStates;
            }
            set
            {
                this._validationStates = value;
            }
        }

        /// <summary>
        /// Property for the Entity Guid
        /// </summary>
        [DataMember]
        [ProtoMember(62)]
        public Guid? EntityGuid
        {
            get { return _entityGuid; }
            set { _entityGuid = value; }
        }

        /// <summary>
        /// Indicates the cross reference id for an entity. Indicates the approved entity id in case the current entity is collaboration and vice versa.
        /// </summary>
        [DataMember]
        [ProtoMember(63)]
        public Int64 CrossReferenceId
        {
            get
            {
                return this._crossReferenceId;
            }
            set
            {
                this._crossReferenceId = value;
            }
        }

        /// <summary>
        /// Indicates entity business conditions
        /// </summary>
        [DataMember]
        [ProtoMember(64)]
        public BusinessConditionStatusCollection BusinessConditions
        {
            get
            {
                if (this._businessConditions == null)
                {
                    this._businessConditions = new BusinessConditionStatusCollection();
                }

                return this._businessConditions;
            }
            set
            {
                this._businessConditions = value;
            }
        }

        /// <summary>
        /// Indicates entity family name for a variant tree
        /// </summary>
        [DataMember]
        [ProtoMember(65)]
        public string EntityFamilyLongName
        {
            get
            {
                return this._entityFamilyLongName;
            }
            set
            {
                this._entityFamilyLongName = value;
            }
        }

        /// <summary>
        /// Indicates entity global family name across parent(including extended families)
        /// </summary>
        [DataMember]
        [ProtoMember(66)]
        public string EntityGlobalFamilyLongName
        {
            get
            {
                return this._entityGlobalFamilyLongName;
            }
            set
            {
                this._entityGlobalFamilyLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the cross reference guid. Indicates the approved entity guid in case the current entity is collaboration and vice versa.
        /// </summary>
        [DataMember]
        [ProtoMember(67)]
        public Guid? CrossReferenceGuid
        {
            get
            {
                return this._crossReferenceGuid;
            }
            set
            {
                this._crossReferenceGuid = value;
            }
        }

        /// <summary>
        /// Property defines State validations
        /// This property has to be filled separate where ever it's required.
        /// </summary>
        public EntityStateValidationCollection EntityStateValidations
        {
            get
            {
                if (this._entityStateValidations == null)
                {
                    this._entityStateValidations = new EntityStateValidationCollection();
                }

                return this._entityStateValidations;
            }
            set
            {
                this._entityStateValidations = value;
            }
        }

        /// <summary>
        /// Specifies list of rule map context ids which are associated with an entity
        /// </summary>
        public Collection<Int32> RuleMapContextIdList
        {
            get
            {
                if (this._ruleMapContextIdList == null)
                {
                    this._ruleMapContextIdList = new Collection<Int32>();
                }

                return this._ruleMapContextIdList;
            }
            set
            {
                this._ruleMapContextIdList = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load entity object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value
        /// <![CDATA[
        /// 	<Entity 
        ///     	Id="4384979" 
        ///     	ObjectId="320207" 
        ///     	Name="P2001" 
        ///     	LongName="P2001" 
        ///     	SKU="P2001" 
        ///     	Path="Bluestem 
        ///     	Brands#@#Product Master#@#!   ! GrandParent Category#@#P2001" 
        ///     	CategoryPath = "!   ! GrandParent Category#@#P2001"
        ///     	BranchLevel="1" 
        ///     	ParentEntityId="4384912" 
        ///     	ParentEntityName="!   ! GrandParent Category" 
        ///     	ParentEntityLongName="!   ! GrandParent Category" 
        ///     	CategoryId="4384912" 
        ///     	CategoryName="!   ! GrandParent Category" 
        ///     	CategoryLongName="!   ! GrandParent Category" 
        ///     	ContainerId="17" 
        ///         ContainerName="Product Master" 
        ///         ContainerLongName="Product Master" 
        ///         OrganizationId="4" 
        ///         OrganizationName="Bluestem Brands" 
        ///         OrganizationLongName="!   ! GrandParent Category" 
        ///         EntityTypeId="7" 
        ///         EntityTypeName="4" 
        ///         EntityTypeLongName="Product" 
        ///         Locale="en_WW" 
        ///         LocaleId="1"
        ///         Action="Read" 
        ///         EffectiveFrom="" 
        ///         EffectiveTo="" 
        ///         EntityLastModTime="" 
        ///         EntityLastModUser="">
        ///         	<Attributes>
        ///             	<Attribute Id="3099" Name="Product ID" LongName="Product ID" ValueRefId="120" AttributeParentId="3128" AttributeParentName="Core Attributes" AttributeParentLongName="Core Attributes" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttributeType="Simple" AttributeModelType="Common" AttributeDataType="String" SourceFlag="O" Action="Update">
        ///                 	<Values SourceFlag="O">
        ///                     	<Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW">P2001</Value>
        ///                     </Values>
        ///                         <Values SourceFlag="I" />
        ///                 </Attribute >
        ///             </Attributes>
        ///         </Entity>
        /// 	]]>   
        /// </param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public void LoadEntityDetail(String valuesAsXml, ObjectSerialization objectSerialization)
        {
            #region Sample Xml
            /*
                  <Entity 
                    Id="4384979" 
                    ObjectId="320207" 
                    Name="P2001" 
                    LongName="P2001" 
                    SKU="P2001" 
                    Path="Bluestem 
                    Brands#@#Product Master#@#!   ! GrandParent Category#@#P2001" 
                    CategoryPath ="!   ! GrandParent Category#@#P2001"
                    BranchLevel="1" 
                    ParentEntityId="4384912" 
                    ParentEntityName="!   ! GrandParent Category" 
                    ParentEntityLongName="!   ! GrandParent Category" 
                    CategoryId="4384912" 
                    CategoryName="!   ! GrandParent Category" 
                    CategoryLongName="!   ! GrandParent Category" 
                    ContainerId="17" 
                    ContainerName="Product Master" 
                    ContainerLongName="Product Master" 
                    OrganizationId="4" 
                    OrganizationName="Bluestem Brands" 
                    OrganizationLongName="!   ! GrandParent Category" 
                    EntityTypeId="7" 
                    EntityTypeName="4" 
                    EntityTypeLongName="Product" 
                    Locale="en_WW" 
                    Action="Read" 
                    EffectiveFrom="" 
                    EffectiveTo="" 
                    EntityLastModTime="" 
                    EntityLastModUser="">
                        <Attributes>
                            <Attribute Id="3099" Name="Product ID" LongName="Product ID" ValueRefId="120" AttributeParentId="3128" AttributeParentName="Core Attributes" AttributeParentLongName="Core Attributes" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttributeType="Simple" AttributeModelType="Common" AttributeDataType="String" SourceFlag="O" Action="Update">
                                <Values SourceFlag="O">
                                    <Value Uom="" ValueRefId="-1" Sequence="0" Locale="en_WW"><![CDATA[P2001]]></Value>
                                </Values>
                                <Values SourceFlag="I" />
                            <Attributes />
                        </Attribute>
                     </Entity>
               */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadEntityDetailForDataStorage(valuesAsXml);
                }
                else if (objectSerialization == ObjectSerialization.DataTransfer)
                {
                    LoadEntityDetailForDataTransfer(valuesAsXml);
                }
                else
                {

                    XmlTextReader reader = null;
                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                            {
                                //Read entity metadata
                                #region Read Entity Metadata

                                if (reader.HasAttributes)
                                {
                                    LoadEntityMetadataFromXml(reader);

                                    reader.Read();
                                }

                                #endregion Read Entity Metadata
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                            {
                                //Read attributes
                                #region Read attributes

                                String attributeXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(attributeXml))
                                {
                                    AttributeCollection attributeCollection = new AttributeCollection(attributeXml);
                                    if (attributeCollection != null)
                                    {
                                        // Based on the serialization type the unique 
                                        if (objectSerialization == ObjectSerialization.External)
                                        {
                                            foreach (Attribute attr in attributeCollection)
                                            {
                                                if (!this.Attributes.Contains(attr.Name, attr.AttributeParentName, attr.Locale))
                                                    this.Attributes.Add(attr);
                                            }
                                        }
                                        else
                                        {
                                            foreach (Attribute attr in attributeCollection)
                                            {
                                                if (!this.Attributes.Contains(attr.Id, attr.Locale))
                                                    this.Attributes.Add(attr);
                                            }

                                        }
                                    }
                                }

                                #endregion Read attributes
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModels")
                            {
                                //Read attribute models
                                #region Read attribute models

                                String attributeModelXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(attributeModelXml))
                                {
                                    AttributeModelCollection attributeModelCollection = new AttributeModelCollection(attributeModelXml);
                                    if (attributeModelCollection != null)
                                    {
                                        foreach (AttributeModel attrModel in attributeModelCollection)
                                        {
                                            if (!this.AttributeModels.Contains(attrModel.Id, attrModel.Locale))
                                                this.AttributeModels.Add(attrModel);
                                        }
                                    }
                                }

                                #endregion Read attributes
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                            {
                                // Read relationships
                                #region Read Relationships
                                String relationshipXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(relationshipXml))
                                {
                                    RelationshipCollection relationshipCollection = new RelationshipCollection(relationshipXml);
                                    if (relationshipCollection != null)
                                    {
                                        foreach (Relationship relationship in relationshipCollection)
                                        {
                                            if (!this.Relationships.Contains(relationship))
                                            {
                                                this.Relationships.Add(relationship);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Hierarchies")
                            {
                                // Read Hierarchy Relationships
                                #region Read Hierarchy Relationships

                                String hierarchyRelationshipXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(hierarchyRelationshipXml))
                                {
                                    HierarchyRelationshipCollection HierarchyRelationshipCollection = new HierarchyRelationshipCollection(hierarchyRelationshipXml);
                                    if (HierarchyRelationshipCollection != null)
                                    {
                                        foreach (HierarchyRelationship hierarchyRelationship in HierarchyRelationshipCollection)
                                        {
                                            if (!this.HierarchyRelationships.Contains(hierarchyRelationship))
                                            {
                                                this.HierarchyRelationships.Add(hierarchyRelationship);
                                            }
                                        }
                                    }
                                }

                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Extensions")
                            {
                                // Read Extension Relationships
                                #region Read Extension Relationships

                                String extensionRelationshipXml = reader.ReadOuterXml();
                                if (!String.IsNullOrEmpty(extensionRelationshipXml))
                                {
                                    ExtensionRelationshipCollection extensionrelationshipCollection = new ExtensionRelationshipCollection(extensionRelationshipXml);
                                    if (extensionrelationshipCollection != null && extensionrelationshipCollection.Count > 0)
                                    {
                                        foreach (ExtensionRelationship extensionrelationship in extensionrelationshipCollection)
                                        {
                                            if (!this.ExtensionRelationships.Contains(extensionrelationship))
                                            {
                                                this.ExtensionRelationships.Add(extensionrelationship);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowStates")
                            {
                                // Read Workflow Information's
                                #region Read Workflow Information's

                                String workflowInformationsXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(workflowInformationsXml))
                                {
                                    WorkflowStateCollection wfStateCollection = new WorkflowStateCollection(workflowInformationsXml);
                                    if (wfStateCollection != null)
                                    {
                                        this.WorkflowStates = wfStateCollection;
                                    }
                                }

                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowActionContext")
                            {
                                // Read Workflow action context Information's
                                #region Read Workflow Information's

                                String workflowActionContextXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(workflowActionContextXml))
                                {
                                    WorkflowActionContext wfActionContext = new WorkflowActionContext(workflowActionContextXml);
                                    if (wfActionContext != null)
                                    {
                                        this.WorkflowActionContext = wfActionContext;
                                    }
                                }

                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityMoveContext")
                            {
                                // Read entity move context Information's
                                #region Read EntityMoveContext Information's

                                String entityMoveContextXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(entityMoveContextXml))
                                {
                                    EntityMoveContext entityMoveContext = new EntityMoveContext(entityMoveContextXml);
                                    if (entityMoveContext != null)
                                    {
                                        this.EntityMoveContext = entityMoveContext;
                                    }
                                }

                                #endregion Read EntityMoveContext Information's
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
                        }

                        #region Update Entity.EntityContext with DataLocales from Attribute in Entity


                        if (this._attributes != null)
                        {
                            //Get distinct list of locales available in entity's attributes.
                            Collection<LocaleEnum> locales = this.Attributes.GetLocaleList();
                            if (locales != null && this.EntityContext.DataLocales.Count < 1)
                            {
                                foreach (LocaleEnum locale in locales)
                                {
                                    this.EntityContext.DataLocales.Add(locale);
                                }
                            }
                        }
                        #endregion Update Entity.EntityContext with DataLocales from Attribute in Entity

                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load entity object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value
        /// <![CDATA[
        /// 	<Entity 
        ///     	Id="4384979" 
        ///     	ObjectId="320207" 
        ///     	Name="P2001" 
        ///     	LName="P2001" 
        ///     	SKU="P2001" 
        ///     	Path="Bluestem 
        ///     	Brands#@#Product Master#@#!   ! GrandParent Category#@#P2001" 
        ///     	CatPath = "!   ! GrandParent Category#@#P2001"
        ///     	BranchLevel="1" 
        ///     	ParentEnId="4384912" 
        ///     	ParentEnName="!   ! GrandParent Category" 
        ///     	ParentEnLName="!   ! GrandParent Category" 
        ///     	CatId="4384912" 
        ///     	CatName="!   ! GrandParent Category" 
        ///     	CatLName="!   ! GrandParent Category" 
        ///     	ContId="17" 
        ///         ContName="Product Master" 
        ///         ContLName="Product Master" 
        ///         OrgId="4" 
        ///         OrgName="Bluestem Brands" 
        ///         OrgLName="!   ! GrandParent Category" 
        ///         EnTypeId="7" 
        ///         EnTypeName="4" 
        ///         EnTypeLName="Product" 
        ///         Locale="en_WW" 
        ///         LocaleId="1"
        ///         Action="Read" 
        ///         EffectiveFrom="" 
        ///         EffectiveTo="" 
        ///         EntityLastModTime="" 
        ///         EntityLastModUser="">
        ///         	<Attributes>
        ///             	<Attribute Id="3099" Name="Product ID" LName="Product ID" ValRefId="120" AttrParentId="3128" AttrParentName="Core Attributes" AttrParentLName="Core Attributes" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttrType="Simple" AttrModelType="Common" AttrDataType="String" SourceFlag="O" Action="Update">
        ///                 	<Values SourceFlag="O">
        ///                     	<Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW">P2001</Value>
        ///                     </Values>
        ///                         <Values SourceFlag="I" />
        ///                 </Attribute >
        ///             </Attributes>
        ///         </Entity>
        /// 	]]>   
        /// </param>
        public void LoadEntityDetailForDataStorage(String valuesAsXml)
        {
            #region Sample Xml
            /*
                Id="4384979" 
              	ObjectId="320207" 
             	Name="P2001" 
            	LName="P2001" 
            	SKU="P2001" 
               	Path="Bluestem 
            	Brands#@#Product Master#@#!   ! GrandParent Category#@#P2001" 
            	CatPath = "!   ! GrandParent Category#@#P2001"
            	BranchLevel="1" 
            	ParentEnId="4384912" 
            	ParentEnName="!   ! GrandParent Category" 
            	ParentEnLName="!   ! GrandParent Category" 
            	CatId="4384912" 
             	CatName="!   ! GrandParent Category" 
            	CatLName="!   ! GrandParent Category" 
               	ContId="17" 
                ContName="Product Master" 
                ContLName="Product Master" 
                OrgId="4" 
                OrgName="Bluestem Brands" 
                OrgLName="!   ! GrandParent Category" 
                EnTypeId="7" 
                EnTypeName="4" 
                EnTypeLName="Product" 
                Locale="en_WW" 
                LocaleId="1"
                Action="Read" 
                EffectiveFrom="" 
                EffectiveTo="" 
                EntityLastModTime="" 
                EntityLastModUser="">
                	<Attributes>
                    	<Attribute Id="3099" Name="Product ID" LName="Product ID" ValRefId="120" AttrParentId="3128" AttrParentName="Core Attributes" AttrParentLName="Core Attributes" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttrType="Simple" AttrModelType="Common" AttrDataType="String" SourceFlag="O" Action="Update">
                        	<Values SourceFlag="O">
                            	<Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW">P2001</Value>
                           </Values>
                                <Values SourceFlag="I" />
                        </Attribute >
                    </Attributes>
               </Entity>
               */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                        {
                            //Read entity metadata
                            #region Read Entity Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("ExUniqueId"))
                                {
                                    this.ExtensionUniqueId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ExtensionUniqueId);
                                }

                                if (reader.MoveToAttribute("ExtnId"))
                                {
                                    this.ExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RefId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ReferenceId);
                                }

                                if (reader.MoveToAttribute("ParentExId"))
                                {
                                    this.ParentExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SKU"))
                                {
                                    this.SKU = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Path"))
                                {
                                    this.Path = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatPath"))
                                {
                                    this.CategoryPath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatLNamePath"))
                                {
                                    this.CategoryLongNamePath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("TargetCategoryPath"))   //This will be used only for RsXml to support reclassification.
                                {

                                    this.EntityMoveContext.TargetCategoryPath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("BranchLevel"))
                                {
                                    this.BranchLevel = (ContainerBranchLevel)reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("ParentEnId"))
                                {
                                    this.ParentEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ParentEntityId);
                                }

                                if (reader.MoveToAttribute("ParentEnTypeId"))
                                {
                                    this.ParentEntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ParentEntityTypeId);
                                }

                                if (reader.MoveToAttribute("ParentEnName"))
                                {
                                    this.ParentEntityName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentEnLName"))
                                {
                                    this.ParentEntityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnId"))
                                {
                                    this.ParentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ParentExtensionEntityId);
                                }

                                if (reader.MoveToAttribute("ParentExEnName"))
                                {
                                    this.ParentExtensionEntityName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnLName"))
                                {
                                    this.ParentExtensionEntityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnExtnId"))
                                {
                                    this.ParentExtensionEntityExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnContId"))
                                {
                                    this.ParentExtensionEntityContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ParentExtensionEntityContainerId);
                                }

                                if (reader.MoveToAttribute("ParentExEnContName"))
                                {
                                    this.ParentExtensionEntityContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnContLName"))
                                {
                                    this.ParentExtensionEntityContainerLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnCatId"))
                                {
                                    this.ParentExtensionEntityCategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ParentExtensionEntityCategoryId);
                                }

                                if (reader.MoveToAttribute("ParentExEnCatName"))
                                {
                                    this.ParentExtensionEntityCategoryName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnCatLName"))
                                {
                                    this.ParentExtensionEntityCategoryLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnCatPath"))
                                {
                                    this.ParentExtensionEntityCategoryPath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ParentExEnCatLNamePath"))
                                {
                                    this.ParentExtensionEntityCategoryLongNamePath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatId"))
                                {
                                    this.CategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.CategoryId);
                                }

                                if (reader.MoveToAttribute("CatName"))
                                {
                                    this.CategoryName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatLName"))
                                {
                                    this.CategoryLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ContainerId);
                                }

                                if (reader.MoveToAttribute("ContName"))
                                {
                                    this.ContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContLName"))
                                {
                                    this.ContainerLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OrgId"))
                                {
                                    this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.OrganizationId);
                                }

                                if (reader.MoveToAttribute("OrgName"))
                                {
                                    this.OrganizationName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OrgLName"))
                                {
                                    this.OrganizationLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EnTypeId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.EntityTypeId);
                                }

                                if (reader.MoveToAttribute("EnTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EnTypeLName"))
                                {
                                    this.EntityTypeLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LocaleId"))
                                {
                                    Int32 localeId = reader.ReadContentAsInt();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    locale = (LocaleEnum)localeId;

                                    if (locale == LocaleEnum.UnKnown)
                                        throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");

                                    this.Locale = locale;
                                }
                                else if (reader.MoveToAttribute("Locale"))
                                {
                                    String strLocale = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                                    if (locale == LocaleEnum.UnKnown)
                                        throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");

                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("SeqNum"))
                                {
                                    this.SequenceNumber = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.SequenceNumber);
                                }

                                if (reader.MoveToAttribute("SourceClass"))
                                {
                                    this.SourceClass = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("IsDirectChange"))
                                {
                                    this.IsDirectChange = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), true);
                                }

                                if (reader.MoveToAttribute("EnTreeIdList"))
                                {
                                    this.EntityTreeIdList = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ExUniqueId"))
                                {
                                    this.ExtensionUniqueId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ExtensionUniqueId);
                                }

                                if (reader.MoveToAttribute("IdPath"))
                                {
                                    this.IdPath = reader.ReadContentAsString();
                                }

                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
                            //Read attributes
                            #region Read attributes

                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                AttributeCollection attributeCollection = new AttributeCollection(attributeXml, false, ObjectSerialization.DataStorage);
                                if (attributeCollection != null)
                                {
                                    // Based on the serialization type the unique 
                                    foreach (Attribute attr in attributeCollection)
                                    {
                                        if (!this.Attributes.Contains(attr.Id, attr.Locale))
                                            this.Attributes.Add(attr);
                                    }
                                }
                            }

                            #endregion Read attributes
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                        {
                            // Read relationships
                            #region Read Relationships
                            String relationshipXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(relationshipXml))
                            {
                                RelationshipCollection relationshipCollection = new RelationshipCollection(relationshipXml, ObjectSerialization.DataStorage);
                                if (relationshipCollection != null)
                                {
                                    foreach (Relationship relationship in relationshipCollection)
                                    {
                                        if (!this.Relationships.Contains(relationship))
                                        {
                                            this.Relationships.Add(relationship);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Hierarchies")
                        {
                            // Read Hierarchy Relationships
                            #region Read Hierarchy Relationships

                            String hierarchyRelationshipXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(hierarchyRelationshipXml))
                            {
                                HierarchyRelationshipCollection HierarchyRelationshipCollection = new HierarchyRelationshipCollection(hierarchyRelationshipXml, ObjectSerialization.DataStorage);
                                if (HierarchyRelationshipCollection != null)
                                {
                                    foreach (HierarchyRelationship hierarchyRelationship in HierarchyRelationshipCollection)
                                    {
                                        if (!this.HierarchyRelationships.Contains(hierarchyRelationship))
                                        {
                                            this.HierarchyRelationships.Add(hierarchyRelationship);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Extensions")
                        {
                            // Read Extension Relationships
                            #region Read Extension Relationships

                            String extensionRelationshipXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(extensionRelationshipXml))
                            {
                                ExtensionRelationshipCollection extensionrelationshipCollection = new ExtensionRelationshipCollection(extensionRelationshipXml, ObjectSerialization.DataStorage);
                                if (extensionrelationshipCollection != null && extensionrelationshipCollection.Count > 0)
                                {
                                    foreach (ExtensionRelationship extensionrelationship in extensionrelationshipCollection)
                                    {
                                        if (!this.ExtensionRelationships.Contains(extensionrelationship))
                                        {
                                            this.ExtensionRelationships.Add(extensionrelationship);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowStates")
                        {
                            // Read Workflow Information's
                            #region Read Workflow Information's

                            String workflowInformationsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(workflowInformationsXml))
                            {
                                WorkflowStateCollection wfStateCollection = new WorkflowStateCollection(workflowInformationsXml, ObjectSerialization.DataStorage);
                                if (wfStateCollection != null)
                                {
                                    this.WorkflowStates = wfStateCollection;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowActionContext")
                        {
                            // Read Workflow action context Information's
                            #region Read Workflow Information's

                            String workflowActionContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(workflowActionContextXml))
                            {
                                WorkflowActionContext wfActionContext = new WorkflowActionContext(workflowActionContextXml, ObjectSerialization.DataStorage);
                                if (wfActionContext != null)
                                {
                                    this.WorkflowActionContext = wfActionContext;
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }

                    #region Update Entity.EntityContext with DataLocales from Attribute in Entity



                    if (this._attributes != null)
                    {
                        //Get distinct list of locales available in entity's attributes.
                        Collection<LocaleEnum> locales = this.Attributes.GetLocaleList();
                        if (locales != null && this.EntityContext.DataLocales.Count < 1)
                        {
                            foreach (LocaleEnum locale in locales)
                            {
                                this.EntityContext.DataLocales.Add(locale);
                            }
                        }
                    }
                    #endregion Update Entity.EntityContext with DataLocales from Attribute in Entity

                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Loads Entity object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        /// <param name="context">Indicates context object which specifies what all data to be converted into object from Xml</param>
        public void LoadEntityFromXml(XmlTextReader reader, EntityConversionContext context)
        {
            if (reader != null)
            {
                while (!(reader.Name == "Entity" && reader.NodeType == XmlNodeType.EndElement))
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Entity":
                                if (reader.HasAttributes)
                                {
                                    LoadEntityMetadataFromXml(reader);
                                }
                                break;
                            case "Attributes":
                                Attributes.LoadAttributeCollectionFromXml(reader);
                                break;
                            case "AttributeModels":
                                AttributeModels.LoadAttributeModelCollectionFromXml(reader);
                                break;
                            case "Hierarchies":
                                HierarchyRelationships.LoadHierarchyRelationshipCollectionFromXml(reader, context);
                                break;
                            case "Relationships":
                                Relationships.LoadRelationshipCollectionFromXml(reader, context);
                                break;
                            case "Extensions":
                                ExtensionRelationships.LoadExtensionRelationshipCollectionFromXml(reader);
                                break;
                            case "WorkflowStates":
                                WorkflowStates.LoadWorkflowStateCollectionFromXml(reader);
                                break;
                            case "WorkflowActionContext":
                                if (reader.HasAttributes)
                                {
                                    WorkflowActionContext.LoadWorkflowActionContextMetadataFromXml(reader);
                                }
                                break;
                            case "EntityMoveContext":
                                if (reader.HasAttributes)
                                {
                                    EntityMoveContext.LoadEntityMoveContextMetadataFromXml(reader);
                                }
                                break;
                        }
                    }

                    reader.Read();
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read Entity object.");
            }
        }

        /// <summary>
        /// Check if entity object has value
        /// </summary>
        /// <param name="id">Entity Id for which we are checking</param>
        /// <returns>true if entity has value otherwise false</returns>
        public bool HasValue(Int32 id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is Entity)
                {
                    Entity objectToBeCompared = obj as Entity;

                    if (this.ExtensionUniqueId != objectToBeCompared.ExtensionUniqueId)
                        return false;

                    if (this.ExternalId != objectToBeCompared.ExternalId)
                        return false;

                    if (this.ParentExternalId != objectToBeCompared.ParentExternalId)
                        return false;

                    if (this.SKU != objectToBeCompared.SKU)
                        return false;

                    if (this.ParentEntityId != objectToBeCompared.ParentEntityId)
                        return false;

                    if (this.ParentEntityName != objectToBeCompared.ParentEntityName)
                        return false;

                    if (this.ParentEntityLongName != objectToBeCompared.ParentEntityLongName)
                        return false;

                    if (this.ParentExtensionEntityId != objectToBeCompared.ParentExtensionEntityId)
                        return false;

                    if (this.ParentExtensionEntityName != objectToBeCompared.ParentExtensionEntityName)
                        return false;

                    if (this.ParentExtensionEntityLongName != objectToBeCompared.ParentExtensionEntityLongName)
                        return false;

                    if (this.ParentExtensionEntityExternalId != objectToBeCompared.ParentExtensionEntityExternalId)
                        return false;

                    if (this.ParentExtensionEntityContainerId != objectToBeCompared.ParentExtensionEntityContainerId)
                        return false;

                    if (this.ParentExtensionEntityContainerName != objectToBeCompared.ParentExtensionEntityContainerName)
                        return false;

                    if (this.ParentExtensionEntityContainerName != objectToBeCompared.ParentExtensionEntityContainerName)
                        return false;

                    if (this.ParentExtensionEntityCategoryId != objectToBeCompared.ParentExtensionEntityCategoryId)
                        return false;

                    if (this.ParentExtensionEntityCategoryName != objectToBeCompared.ParentExtensionEntityCategoryName)
                        return false;

                    if (this.ParentExtensionEntityCategoryLongName != objectToBeCompared.ParentExtensionEntityCategoryLongName)
                        return false;

                    if (this.ParentExtensionEntityCategoryPath != objectToBeCompared.ParentExtensionEntityCategoryPath)
                        return false;

                    if (this.ParentExtensionEntityCategoryLongNamePath != objectToBeCompared.ParentExtensionEntityCategoryLongNamePath)
                        return false;

                    if (this.CategoryId != objectToBeCompared.CategoryId)
                        return false;

                    if (this.CategoryName != objectToBeCompared.CategoryName)
                        return false;

                    if (this.CategoryLongName != objectToBeCompared.CategoryLongName)
                        return false;

                    if (this.ContainerId != objectToBeCompared.ContainerId)
                        return false;

                    if (this.ContainerName != objectToBeCompared.ContainerName)
                        return false;

                    if (this.ContainerLongName != objectToBeCompared.ContainerLongName)
                        return false;

                    if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                        return false;

                    if (this.EntityTypeName != objectToBeCompared.EntityTypeName)
                        return false;

                    if (this.EntityTypeLongName != objectToBeCompared.EntityTypeLongName)
                        return false;

                    if (this.OrganizationId != objectToBeCompared.OrganizationId)
                        return false;

                    if (this.OrganizationName != objectToBeCompared.OrganizationName)
                        return false;

                    if (this.ShowMaster != objectToBeCompared.ShowMaster)
                        return false;

                    if (this.Path != objectToBeCompared.Path)
                        return false;

                    if (this.CategoryPath != objectToBeCompared.CategoryPath)
                        return false;

                    if (this.CategoryLongNamePath != objectToBeCompared.CategoryLongNamePath)
                        return false;

                    if (this.IdPath != objectToBeCompared.IdPath)
                        return false;

                    if (this.BranchLevel != objectToBeCompared.BranchLevel)
                        return false;

                    if (this.SequenceNumber != objectToBeCompared.SequenceNumber)
                    {
                        return false;
                    }

                    if (!this.Attributes.Equals(objectToBeCompared.Attributes))
                        return false;

                    if (!this.HierarchyRelationships.Equals(objectToBeCompared.HierarchyRelationships))
                        return false;

                    if (!this.ExtensionRelationships.Equals(objectToBeCompared.ExtensionRelationships))
                        return false;

                    if (this.ActivityLogId != objectToBeCompared.ActivityLogId)
                        return false;

                    if (this.IsDirectChange != objectToBeCompared.IsDirectChange)
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
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() ^ this.ExtensionUniqueId.GetHashCode() ^ this.ExternalId.GetHashCode() ^ this.ParentExternalId.GetHashCode() ^ this.SKU.GetHashCode() ^ this.ParentEntityId.GetHashCode() ^ this.ParentEntityName.GetHashCode() ^ this.ParentEntityLongName.GetHashCode() ^ this.ParentExtensionEntityId.GetHashCode() ^ this.ParentExtensionEntityName.GetHashCode() ^ this.ParentExtensionEntityLongName.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.CategoryName.GetHashCode() ^ this.CategoryLongName.GetHashCode() ^
                this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^ this.ContainerLongName.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.EntityTypeName.GetHashCode() ^ this.EntityTypeLongName.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.OrganizationName.GetHashCode() ^ this.ShowMaster.GetHashCode() ^ this.Path.GetHashCode() ^ this.CategoryPath.GetHashCode() ^ this.CategoryLongNamePath.GetHashCode() ^ this.IdPath.GetHashCode() ^ this.BranchLevel.GetHashCode() ^ this.Attributes.GetHashCode() ^ this.Relationships.GetHashCode() ^ this.HierarchyRelationships.GetHashCode() ^ this.ExtensionRelationships.GetHashCode() ^
                this.ParentExtensionEntityExternalId.GetHashCode() ^ this.ParentExtensionEntityContainerId.GetHashCode() ^ this.ParentExtensionEntityContainerName.GetHashCode() ^ this.ParentExtensionEntityContainerLongName.GetHashCode() ^ this.ParentExtensionEntityCategoryId.GetHashCode() ^ this.ParentExtensionEntityCategoryName.GetHashCode() ^ this.ParentExtensionEntityCategoryLongName.GetHashCode() ^ this.ParentExtensionEntityCategoryPath.GetHashCode() ^ this.ParentExtensionEntityCategoryLongNamePath.GetHashCode() ^ this.SequenceNumber.GetHashCode() ^ this.PermissionSet.GetHashCode() ^ this.ActivityLogId.GetHashCode() ^ this.IsDirectChange.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return String.Format("EntityId: {0}, Name: {1}, LongName: {2}, EntityType: {3}, CategoryPath: {4} and Container: {5}", this.Id, this.Name, this.LongName, this.EntityTypeName, this.CategoryPath, this.ContainerName);
        }

        /// <summary>
        /// Compare Entity with current entity.
        /// This method will compare entity, its attributes and Values.  If current entity has more attributes than entity to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subSetEntity">Entity to be compared with current entity</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(Entity subSetEntity)
        {
            if (this.Name != subSetEntity.Name)
                return false;

            if (this.LongName != subSetEntity.LongName)
                return false;

            if (this.Locale != subSetEntity.Locale)
                return false;

            if (this.ExternalId != subSetEntity.ExternalId)
                return false;

            if (this.SKU != subSetEntity.SKU)
                return false;

            if (this.ParentEntityName != subSetEntity.ParentEntityName)
                return false;

            if (this.ParentEntityLongName != subSetEntity.ParentEntityLongName)
                return false;

            if (this.ParentExtensionEntityName != subSetEntity.ParentExtensionEntityName)
                return false;

            if (this.ParentExtensionEntityLongName != subSetEntity.ParentExtensionEntityLongName)
                return false;

            if (this.ParentExtensionEntityExternalId != subSetEntity.ParentExtensionEntityExternalId)
                return false;

            if (this.ParentExtensionEntityContainerName != subSetEntity.ParentExtensionEntityContainerName)
                return false;

            if (this.ParentExtensionEntityContainerName != subSetEntity.ParentExtensionEntityContainerName)
                return false;

            if (this.ParentExtensionEntityCategoryName != subSetEntity.ParentExtensionEntityCategoryName)
                return false;

            if (this.ParentExtensionEntityCategoryLongName != subSetEntity.ParentExtensionEntityCategoryLongName)
                return false;

            if (this.ParentExtensionEntityCategoryPath != subSetEntity.ParentExtensionEntityCategoryPath)
                return false;

            if (this.ParentExtensionEntityCategoryLongNamePath != subSetEntity.ParentExtensionEntityCategoryLongNamePath)
                return false;

            if (this.CategoryName != subSetEntity.CategoryName)
                return false;

            if (this.CategoryLongName != subSetEntity.CategoryLongName)
                return false;

            if (this.ContainerName != subSetEntity.ContainerName)
                return false;

            if (this.ContainerLongName != subSetEntity.ContainerLongName)
                return false;

            if (this.EntityTypeName != subSetEntity.EntityTypeName)
                return false;

            if (this.EntityTypeLongName != subSetEntity.EntityTypeLongName)
                return false;

            if (this.OrganizationName != subSetEntity.OrganizationName)
                return false;

            if (this.ShowMaster != subSetEntity.ShowMaster)
                return false;

            if (this.Path != subSetEntity.Path)
                return false;

            if (this.CategoryPath != subSetEntity.CategoryPath)
                return false;

            if (this.CategoryLongNamePath != subSetEntity.CategoryLongNamePath)
                return false;

            if (this.BranchLevel != subSetEntity.BranchLevel)
                return false;

            if (this.SequenceNumber != subSetEntity.SequenceNumber)
            {
                return false;
            }

            if (!this.Attributes.IsSuperSetOf(subSetEntity.Attributes))
                return false;

            if (!this.Relationships.IsSuperSetOf(subSetEntity.Relationships))
                return false;

            if (!this.AttributeModels.IsSuperSetOf(subSetEntity.AttributeModels))
                return false;

            if (!this.HierarchyRelationships.IsSuperSetOf(subSetEntity.HierarchyRelationships))
                return false;

            return true;
        }

        /// <summary>
        /// This method will compare entity, its attributes and Values.  If current entity has more attributes than entity to be compared, extra attributes will be ignored.
        ///If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subSetEntity"></param>
        /// <param name="compareAttributeIds"></param>
        /// <param name="compareRelationshipsIds"></param>
        /// <param name="compareHierarchyRelationshipsIds"></param>
        /// <param name="compareExtensionRelationshipsIds"></param>
        /// <returns></returns>
        public EntityOperationResult GetSuperSetOperationResult(Entity subSetEntity, Boolean compareAttributeIds = false, Boolean compareRelationshipsIds = false, Boolean compareHierarchyRelationshipsIds = false, Boolean compareExtensionRelationshipsIds = false)
        {
            var entityOperationResult = new EntityOperationResult();
            entityOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful; //if no mismatch found operation result is sucessful

            #region compare properties

            Utility.BusinessObjectPropertyCompare("Name", this.Name, subSetEntity.Name, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("LongName", this.LongName, subSetEntity.LongName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("Locale", Convert.ToInt16(this.Locale), Convert.ToInt16(subSetEntity.Locale), entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ExternalId", this.ExternalId, subSetEntity.ExternalId, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("SKU", this.SKU, subSetEntity.SKU, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentEntityName", this.ParentEntityName, subSetEntity.ParentEntityName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentEntityLongName", this.ParentEntityLongName, subSetEntity.ParentEntityLongName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityName", this.ParentExtensionEntityName, subSetEntity.ParentExtensionEntityName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityLongName", this.ParentExtensionEntityLongName, subSetEntity.ParentExtensionEntityLongName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityExternalId", this.ParentExtensionEntityExternalId, subSetEntity.ParentExtensionEntityExternalId, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityContainerName", this.ParentExtensionEntityContainerName, subSetEntity.ParentExtensionEntityContainerName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityCategoryName", this.ParentExtensionEntityCategoryName, subSetEntity.ParentExtensionEntityCategoryName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityCategoryLongName", this.ParentExtensionEntityCategoryLongName, subSetEntity.ParentExtensionEntityCategoryLongName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityCategoryPath", this.ParentExtensionEntityCategoryPath, subSetEntity.ParentExtensionEntityCategoryPath, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ParentExtensionEntityCategoryLongNamePath", this.ParentExtensionEntityCategoryLongNamePath, subSetEntity.ParentExtensionEntityCategoryLongNamePath, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("CategoryName", this.CategoryName, subSetEntity.CategoryName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("CategoryLongName", this.CategoryLongName, subSetEntity.CategoryLongName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ContainerName", this.ContainerName, subSetEntity.ContainerName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ContainerLongName", this.ContainerLongName, subSetEntity.ContainerLongName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("EntityTypeName", this.EntityTypeName, subSetEntity.EntityTypeName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("EntityTypeLongName", this.EntityTypeLongName, subSetEntity.EntityTypeLongName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("OrganizationName", this.OrganizationName, subSetEntity.OrganizationName, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("ShowMaster", this.ShowMaster.ToString(), subSetEntity.ShowMaster.ToString(), entityOperationResult);

            Utility.BusinessObjectPropertyCompare("Path", this.Path, subSetEntity.Path, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("CategoryPath", this.CategoryPath, subSetEntity.CategoryPath, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("CategoryLongNamePath", this.CategoryLongNamePath, subSetEntity.CategoryLongNamePath, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("BranchLevel", Convert.ToInt16(this.BranchLevel), Convert.ToInt16(this.BranchLevel), entityOperationResult);

            Utility.BusinessObjectPropertyCompare("CategoryLongNamePath", this.CategoryLongNamePath, subSetEntity.CategoryLongNamePath, entityOperationResult);

            Utility.BusinessObjectPropertyCompare("SequenceNumber", this.SequenceNumber, subSetEntity.SequenceNumber, entityOperationResult);

            #endregion compare properties

            #region compare attributes

            Utility.BusinessObjectAttributeOperationResultCompare(this.Attributes, subSetEntity.Attributes, entityOperationResult);

            #endregion compare attributes

            #region compare relationships

            Utility.BusinessObjectRelationshipOperationResultCompare(this.Relationships, subSetEntity.Relationships, entityOperationResult, compareRelationshipsIds);

            #endregion compare relationships

            #region compare attribute models

            Utility.BusinessObjectAttributeOperationResultCompare(this.AttributeModels, subSetEntity.AttributeModels, entityOperationResult);

            #endregion compare attribute models

            #region compare hierarchy relationships

            Utility.BusinessObjectHierarchyRelationshipOperationResultCompare(this.HierarchyRelationships, subSetEntity.HierarchyRelationships, entityOperationResult, compareHierarchyRelationshipsIds);

            #endregion compare hierarchy relationships

            #region compare extension relationship

            Utility.BusinessObjectExtensionRelationshipOperationResultCompare(this.ExtensionRelationships, subSetEntity.ExtensionRelationships, entityOperationResult, compareExtensionRelationshipsIds);

            #endregion compare extension relationship

            entityOperationResult.RefreshOperationResultStatus();

            return entityOperationResult;
        }

        #region IEntity Members

        #region ToXml methods

        /// <summary>
        /// Get XML representation of Entity object
        /// </summary>
        /// <returns>XML representation of Entity object</returns>
        public override String ToXml()
        {
            String entityXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Entity");

            ConvertEntityMetadataToXml(xmlWriter);

            ConvertSourceInfoToXml(xmlWriter);

            #region write entity attributes for Full attribute Xml

            if (this._attributes != null)
                xmlWriter.WriteRaw(this.Attributes.ToXml());

            #endregion write entity attributes for Full attribute Xml

            #region Write Hierarchy Relationships

            if (this._hierarchyRelationships != null)
                xmlWriter.WriteRaw(this.HierarchyRelationships.ToXml());

            #endregion Write Hierarchy Relationships

            #region Write Relationships

            if (this._relationships != null)
                xmlWriter.WriteRaw(this.Relationships.ToXml());

            #endregion Write Relationships

            #region Write Extensions

            if (this._extensionRelationships != null)
                xmlWriter.WriteRaw(this.ExtensionRelationships.ToXml());

            #endregion Write Extensions

            #region Write Workflow Informations

            if (this._workflowStates != null)
                xmlWriter.WriteRaw(this.WorkflowStates.ToXml());

            if (this._workflowActionContext != null)
                xmlWriter.WriteRaw(this.WorkflowActionContext.ToXml());

            #endregion Write Workflow Informations

            #region Write validation state

            if (this._validationStates != null)
            {
                xmlWriter.WriteRaw(this.ValidationStates.ToXml());
            }

            #endregion Write validation state

            #region Write State Validations

            if(this._entityStateValidations != null)
            {
                xmlWriter.WriteRaw(this._entityStateValidations.ToXml());
            }

            #endregion Write State Validations

            #region Write Business Conditions

            if (this._businessConditions != null)
            {
                xmlWriter.WriteRaw(this.BusinessConditions.ToXml());
            }

            #endregion Write Business Conditions

            //Entity node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            entityXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityXml;
        }

        /// <summary>
        /// Get XML representation of Entity object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>XML representation of Entity object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return ToXml(objectSerialization, false);
        }

        /// <summary>
        /// Get XML representation of Entity object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="serializeOnlyRootElements">
        /// Whether to serialize whole Entity object including attributes and relationships or serialize only entity metadata.
        /// <para>
        /// if true then returns only entity metadata. If false then returns attributes also,
        /// </para>
        /// </param>
        /// <returns>XML representation of Entity object</returns>
        public String ToXml(ObjectSerialization objectSerialization, Boolean serializeOnlyRootElements)
        {
            String entityXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                return this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                xmlWriter.WriteStartElement("Entity");

                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    #region write entity meta data for ProcessingOnly Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("ExtnId", this.ExternalId);
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LName", this.LongName);
                    xmlWriter.WriteAttributeString("RefId", this.ReferenceId.ToString());
                    xmlWriter.WriteAttributeString("SKU", this.SKU);
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("CatPath", this.CategoryPath);
                    xmlWriter.WriteAttributeString("CatLNamePath", this.CategoryLongNamePath);
                    xmlWriter.WriteAttributeString("BranchLevel", ((Int32)this.BranchLevel).ToString());
                    xmlWriter.WriteAttributeString("ParentEnId", this.ParentEntityId.ToString());
                    xmlWriter.WriteAttributeString("ParentEnTypeId", this.ParentEntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("ParentExtnId", this.ParentExternalId);
                    xmlWriter.WriteAttributeString("ParentEnName", this.ParentEntityName);
                    xmlWriter.WriteAttributeString("ParentEnLName", this.ParentEntityLongName);
                    xmlWriter.WriteAttributeString("ParentExEnId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("ParentExEnName", this.ParentExtensionEntityName);
                    xmlWriter.WriteAttributeString("ParentExEnLName", this.ParentExtensionEntityLongName);
                    xmlWriter.WriteAttributeString("ParentExEnExtnId", this.ParentExtensionEntityExternalId);
                    xmlWriter.WriteAttributeString("ParentExEnContId", this.ParentExtensionEntityContainerId.ToString());
                    xmlWriter.WriteAttributeString("ParentExEnContName", this.ParentExtensionEntityContainerName);
                    xmlWriter.WriteAttributeString("ParentExEnContLName", this.ParentExtensionEntityContainerLongName);
                    xmlWriter.WriteAttributeString("ParentExEnCatId", this.ParentExtensionEntityCategoryId.ToString());
                    xmlWriter.WriteAttributeString("ParentExEnCatName", this.ParentExtensionEntityCategoryName);
                    xmlWriter.WriteAttributeString("ParentExEnCatLName", this.ParentExtensionEntityCategoryLongName);
                    xmlWriter.WriteAttributeString("ParentExEnCatPath", this.ParentExtensionEntityCategoryPath);
                    xmlWriter.WriteAttributeString("ParentExEnCatLNamePath", this.ParentExtensionEntityCategoryLongNamePath);
                    xmlWriter.WriteAttributeString("CatId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("CatName", this.CategoryName);
                    xmlWriter.WriteAttributeString("CatLName", this.CategoryLongName);
                    xmlWriter.WriteAttributeString("ContId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContName", this.ContainerName);
                    xmlWriter.WriteAttributeString("ContLName", this.ContainerLongName);
                    xmlWriter.WriteAttributeString("OrgId", this.OrganizationId.ToString());
                    xmlWriter.WriteAttributeString("OrgName", this.OrganizationName);
                    xmlWriter.WriteAttributeString("OrgLName", this.OrganizationLongName);
                    xmlWriter.WriteAttributeString("EnTypeId", this.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("EnTypeName", this.EntityTypeName);
                    xmlWriter.WriteAttributeString("EnTypeLName", this.EntityTypeLongName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("SeqNum", this.SequenceNumber.ToString());
                    xmlWriter.WriteAttributeString("SourceClass", this.SourceClass.ToString());
                    xmlWriter.WriteAttributeString("EffectiveFrom", "");
                    xmlWriter.WriteAttributeString("EffectiveTo", "");
                    xmlWriter.WriteAttributeString("EntityLastModTime", "");
                    xmlWriter.WriteAttributeString("EntityLastModUser", "");
                    xmlWriter.WriteAttributeString("IsDirectChange", this.IsDirectChange.ToString());
                    xmlWriter.WriteAttributeString("EnTreeIdList", this.EntityTreeIdList);
                    xmlWriter.WriteAttributeString("ExUniqueId", this.ExtensionUniqueId.ToString());
                    xmlWriter.WriteAttributeString("IdPath", this.IdPath);

                    #endregion write entity meta data for ProcessingOnly Xml
                }
                else if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write entity meta data for ProcessingOnly Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    //xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);
                    xmlWriter.WriteAttributeString("SKU", this.SKU);
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath);
                    //xmlWriter.WriteAttributeString("BranchLevel", ((Int32)this.BranchLevel).ToString());
                    xmlWriter.WriteAttributeString("ParentEntityId", this.ParentEntityId.ToString());
                    //xmlWriter.WriteAttributeString("ParentEntityName", this.ParentEntityName);
                    //xmlWriter.WriteAttributeString("ParentEntityLongName", this.ParentEntityLongName);
                    xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                    //xmlWriter.WriteAttributeString("CategoryName", this.CategoryName);
                    //xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName);
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    //xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
                    //xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName);
                    //xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
                    //xmlWriter.WriteAttributeString("OrganizationName", this.OrganizationName);
                    //xmlWriter.WriteAttributeString("OrganizationLongName", this.ParentEntityLongName);
                    xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                    //xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
                    //xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName);
                    xmlWriter.WriteAttributeString("LocaleId", ((Int32)this.Locale).ToString()); // Pass locale id instead of name while sending for processing

                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    //xmlWriter.WriteAttributeString("EffectiveFrom", "");
                    //xmlWriter.WriteAttributeString("EffectiveTo", "");
                    //xmlWriter.WriteAttributeString("EntityLastModTime", "");
                    //xmlWriter.WriteAttributeString("EntityLastModUser", "");

                    #endregion write entity meta data for ProcessingOnly Xml
                }
                else if (objectSerialization == ObjectSerialization.UIRender)
                {
                    #region write entity meta data for Rendering Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    //xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);
                    //xmlWriter.WriteAttributeString("SKU", this.SKU);
                    xmlWriter.WriteAttributeString("Path", this.Path);
                    xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath);
                    xmlWriter.WriteAttributeString("BranchLevel", ((Int32)this.BranchLevel).ToString());
                    xmlWriter.WriteAttributeString("ParentEntityId", this.ParentEntityId.ToString());
                    //xmlWriter.WriteAttributeString("ParentEntityName", this.ParentEntityName);
                    //xmlWriter.WriteAttributeString("ParentEntityLongName", this.ParentEntityLongName);
                    xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                    //xmlWriter.WriteAttributeString("CategoryName", this.CategoryName);
                    xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName);
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    //xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
                    //xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName);
                    xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
                    //xmlWriter.WriteAttributeString("OrganizationName", this.OrganizationName);
                    xmlWriter.WriteAttributeString("OrganizationLongName", this.ParentEntityLongName);
                    xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                    //xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
                    //xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    //xmlWriter.WriteAttributeString("EffectiveFrom", "");
                    //xmlWriter.WriteAttributeString("EffectiveTo", "");
                    //xmlWriter.WriteAttributeString("EntityLastModTime", "");
                    //xmlWriter.WriteAttributeString("EntityLastModUser", "");

                    #endregion write entity meta data for Rendering Xml
                }
                else if (objectSerialization == ObjectSerialization.External)
                {
                    #region Write entity meta data for External

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("ExternalId", String.IsNullOrEmpty(this.ExternalId) ? this.Name : this.ExternalId);
                    //xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);

                    xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
                    //xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName);

                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
                    //xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName);
                    xmlWriter.WriteAttributeString("OrganizationName", this.OrganizationName);

                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("CategoryName", this.CategoryName);
                    xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName);
                    xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath);
                    xmlWriter.WriteAttributeString("CategoryLongNamePath", this.CategoryLongNamePath);

                    xmlWriter.WriteAttributeString("ParentEntityId", this.ParentEntityId.ToString());
                    xmlWriter.WriteAttributeString("ParentExternalId", String.IsNullOrEmpty(this.ParentExternalId) ? this.ParentEntityName : this.ParentExternalId);
                    //xmlWriter.WriteAttributeString("ParentEntityName", this.ParentEntityName);
                    //xmlWriter.WriteAttributeString("ParentEntityLongName", this.ParentEntityLongName);

                    xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("ParentExtensionEntityExternalId", this.ParentExtensionEntityExternalId);
                    //xmlWriter.WriteAttributeString("ParentExtensionEntityName", this.ParentExtensionEntityName.ToString());

                    xmlWriter.WriteAttributeString("ParentExtensionEntityContainerId", this.ParentExtensionEntityContainerId.ToString());
                    xmlWriter.WriteAttributeString("ParentExtensionEntityContainerName", this.ParentExtensionEntityContainerName);
                    //xmlWriter.WriteAttributeString("ParentExtensionEntityContainerLongName", this.ParentExtensionEntityContainerLongName);
                    xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryId", this.ParentExtensionEntityCategoryId.ToString());
                    //xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryName", this.ParentExtensionEntityCategoryName);
                    //xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryLongName", this.ParentExtensionEntityCategoryLongName);
                    xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryPath", this.ParentExtensionEntityCategoryPath);
                    xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryLongNamePath", this.ParentExtensionEntityCategoryLongNamePath);

                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", ValueTypeHelper.GetActionString(this.Action));

                    #endregion Write entity meta data for External
                }
                else if (objectSerialization == ObjectSerialization.DataTransfer)
                {
                    #region write entity meta data for DataTransfer Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("N", this.Name);
                    xmlWriter.WriteAttributeString("LN", this.LongName);
                    xmlWriter.WriteAttributeString("CatLNP", this.CategoryLongNamePath);
                    xmlWriter.WriteAttributeString("PEId", this.ParentEntityId.ToString());
                    xmlWriter.WriteAttributeString("PETId", this.ParentEntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("PELN", this.ParentEntityLongName);
                    xmlWriter.WriteAttributeString("PExEId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("PExELN", this.ParentExtensionEntityLongName);
                    xmlWriter.WriteAttributeString("CatId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("CatLN", this.CategoryLongName);
                    xmlWriter.WriteAttributeString("ContId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContLN", this.ContainerLongName);
                    xmlWriter.WriteAttributeString("OId", this.OrganizationId.ToString());
                    xmlWriter.WriteAttributeString("OLN", this.OrganizationLongName);
                    xmlWriter.WriteAttributeString("ETId", this.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("ETLN", this.EntityTypeLongName);
                    xmlWriter.WriteAttributeString("L", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("A", this.Action.ToString());
                    xmlWriter.WriteAttributeString("PS", Utility.GetPermissionsAsString(this.PermissionSet));

                    #endregion write entity meta data for DataTransfer Xml
                }

                if (!serializeOnlyRootElements)
                {
                    #region Write entity attribute

                    //Attributes node start
                    xmlWriter.WriteStartElement("Attributes");

                    //Get Xml representation of only those attributes which are changed if Serialization option is Processing Only
                    AttributeCollection attributesForToXml = this.Attributes;
                    bool hasAttributeChanges = false;

                    if (objectSerialization == ObjectSerialization.ProcessingOnly)
                    {
                        attributesForToXml = Utility.GetAttributesForProcessing(this.Attributes, out hasAttributeChanges);
                    }

                    if (attributesForToXml != null)
                    {
                        foreach (Attribute attribute in attributesForToXml)
                        {
                            if (objectSerialization == ObjectSerialization.UIRender)
                            {
                                xmlWriter.WriteRaw(attribute.ToXml(objectSerialization, this.Locale));
                            }
                            else
                            {
                                xmlWriter.WriteRaw(attribute.ToXml(objectSerialization));
                            }
                        }
                    }

                    //Attributes node end
                    xmlWriter.WriteEndElement();

                    #endregion write entity attribute

                    #region Write Hierarchy Relationships

                    if (this._hierarchyRelationships != null)
                    {
                        xmlWriter.WriteRaw(this.HierarchyRelationships.ToXml(objectSerialization));
                    }

                    #endregion

                    #region Write Relationships

                    if (this._relationships != null)
                    {
                        xmlWriter.WriteRaw(this.Relationships.ToXml(objectSerialization));
                    }

                    #endregion

                    #region Write Extensions

                    if (this._extensionRelationships != null)
                    {
                        xmlWriter.WriteRaw(this.ExtensionRelationships.ToXml(objectSerialization));
                    }

                    #endregion

                    #region Write WorkFlow Informations

                    if (this.WorkflowStates != null && this.WorkflowStates.Count > 0 && objectSerialization != ObjectSerialization.External && objectSerialization != ObjectSerialization.DataTransfer)
                    {
                        xmlWriter.WriteRaw(this.WorkflowStates.ToXml(objectSerialization));
                    }

                    if (this._workflowActionContext != null && objectSerialization != ObjectSerialization.External && objectSerialization != ObjectSerialization.DataTransfer)
                    {
                        xmlWriter.WriteRaw(this.WorkflowActionContext.ToXml(objectSerialization));
                    }

                    #endregion

                    #region Write validation state

                    if (this._validationStates != null)
                    {
                        #region Entity State Info

                        xmlWriter.WriteStartElement("ValidationStates");

                        xmlWriter.WriteStartElement("ValidationState");
                        xmlWriter.WriteAttributeString("Name", "IsSelfValid");
                        xmlWriter.WriteAttributeString("Value", this._validationStates.IsSelfValid.ToString());

                        PopulateEntityStateValidationBasedOnSystemAttribute(xmlWriter, this._entityStateValidations, SystemAttributes.EntitySelfValid);

                        xmlWriter.WriteEndElement(); //EntityState

                        xmlWriter.WriteStartElement("ValidationState");
                        xmlWriter.WriteAttributeString("Name", "IsMetaDataValid");
                        xmlWriter.WriteAttributeString("Value", this._validationStates.IsMetaDataValid.ToString());

                        PopulateEntityStateValidationBasedOnSystemAttribute(xmlWriter, this._entityStateValidations, SystemAttributes.EntityMetaDataValid);

                        xmlWriter.WriteEndElement(); //EntityState

                        xmlWriter.WriteStartElement("ValidationState");
                        xmlWriter.WriteAttributeString("Name", "IsCommonAttributesValid");
                        xmlWriter.WriteAttributeString("Value", this._validationStates.IsCommonAttributesValid.ToString());

                        PopulateEntityStateValidationBasedOnSystemAttribute(xmlWriter, this._entityStateValidations, SystemAttributes.EntityCommonAttributesValid);

                        xmlWriter.WriteEndElement(); //EntityState

                        xmlWriter.WriteStartElement("ValidationState");
                        xmlWriter.WriteAttributeString("Name", "IsCategoryAttributesValid");
                        xmlWriter.WriteAttributeString("Value", this._validationStates.IsCategoryAttributesValid.ToString());

                        PopulateEntityStateValidationBasedOnSystemAttribute(xmlWriter, this._entityStateValidations, SystemAttributes.EntityCategoryAttributesValid);

                        xmlWriter.WriteEndElement(); //EntityState

                        xmlWriter.WriteStartElement("ValidationState");
                        xmlWriter.WriteAttributeString("Name", "IsRelationshipsValid");
                        xmlWriter.WriteAttributeString("Value", this._validationStates.IsRelationshipsValid.ToString());

                        PopulateEntityStateValidationBasedOnSystemAttribute(xmlWriter, this._entityStateValidations, SystemAttributes.EntityRelationshipsValid);

                        xmlWriter.WriteEndElement(); //EntityState

                        xmlWriter.WriteStartElement("ValidationState");
                        xmlWriter.WriteAttributeString("Name", "IsEntityVariantValid");
                        xmlWriter.WriteAttributeString("Value", this._validationStates.IsEntityVariantValid.ToString());

                        PopulateEntityStateValidationBasedOnSystemAttribute(xmlWriter, this._entityStateValidations, SystemAttributes.EntityVariantValid);

                        xmlWriter.WriteEndElement(); //EntityState

                        xmlWriter.WriteStartElement("ValidationState");
                        xmlWriter.WriteAttributeString("Name", "IsExtensionsValid");
                        xmlWriter.WriteAttributeString("Value", this._validationStates.IsExtensionsValid.ToString());

                        PopulateEntityStateValidationBasedOnSystemAttribute(xmlWriter, this._entityStateValidations, SystemAttributes.EntityExtensionsValid);

                        xmlWriter.WriteEndElement(); //EntityState

                        xmlWriter.WriteEndElement(); //EntityStateInfo

                        #endregion Entity State Info
                    }

                    #endregion Write validation state

                    #region Write Business Conditions

                    if (this._businessConditions != null)
                    {
                        xmlWriter.WriteRaw(this.BusinessConditions.ToXml());
                    }

                    #endregion Write Business Conditions
                }

                //Entity node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                entityXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return entityXml;
        }

        /// <summary>
        /// Converts given entity object into xml based on passed entity conversion context object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml representation of Entity</param>
        /// <param name="context">Indicates context object which specifies what all data of entity to be converted into Xml</param>
        public void ConvertEntityToXml(XmlTextWriter xmlWriter, EntityConversionContext context)
        {
            if (xmlWriter != null)
            {
                //Entity node start
                xmlWriter.WriteStartElement("Entity");

                ConvertEntityMetadataToXml(xmlWriter);

                ConvertSourceInfoToXml(xmlWriter);

                if (_attributes != null)
                {
                    _attributes.ConvertAttributeCollectionToXml(xmlWriter);
                }

                if (_hierarchyRelationships != null)
                {
                    _hierarchyRelationships.ConvertHierarchyRelationshipCollectionToXml(xmlWriter, context);
                }

                if (_relationships != null)
                {
                    _relationships.ConvertRelationshipCollectionToXml(xmlWriter);
                }

                if (_extensionRelationships != null)
                {
                    _extensionRelationships.ConvertExtensionRelationshipCollectionToXml(xmlWriter);
                }

                if (_workflowStates != null)
                {
                    _workflowStates.ConvertWorkflowStateCollectionToXml(xmlWriter);
                }

                if (_workflowActionContext != null)
                {
                    _workflowActionContext.ConvertWorkflowActionContextToXml(xmlWriter);
                }

                //Entity node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write Entity object.");
            }
        }

        #endregion ToXml methods

        #region Attribute Get for current entity

        /// <summary>
        /// Gets the attributes belonging to the Entity
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttributeCollection GetAttributes()
        {
            if (this._attributes == null)
            {
                return null;
            }
            return (IAttributeCollection)this.Attributes;
        }

        /// <summary>
        /// Get attributes having specific AttributeModelType from current entity's attributes
        /// </summary>
        /// <param name="attributeModelType">AttributeModelType of which attributes are to be fetched from current entity's attributes</param>
        /// <returns>Attribute collection interface. Attributes in this collection are having specified AttributeModelType</returns>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttributeCollection GetAttributes(AttributeModelType attributeModelType)
        {
            if (this._attributes == null)
            {
                return null;
            }
            return this.Attributes.GetAttributes(attributeModelType);
        }

        /// <summary>
        /// Gets attribute(s) with specified attribute short Name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttributeCollection GetAttributes(String attributeShortName)
        {
            if (this._attributes == null)
            {
                return null;
            }
            return this.Attributes.GetAttributes(attributeShortName);
        }

        /// <summary>
        /// Gets attribute with specified attribute short Name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute in Entity's Locale</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttribute GetAttribute(String attributeShortName)
        {
            return this.GetAttribute(attributeShortName, this.Locale);
        }

        /// <summary>
        /// Gets attribute with specified attribute short Name and parent name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Specifies Locale in which Attributes should be returned</param>
        /// <returns>Attribute in requested locale</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(String attributeShortName, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                return null;
            }
            return this.Attributes.GetAttribute(attributeShortName, locale);
        }

        /// <summary>
        /// Gets attribute with specified attribute short Name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Specifies Name of an attribute to search in current entity's attributes</param>
        /// <param name="attributeParentName">Specifies Parent Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute in Entity Locale</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(String attributeShortName, String attributeParentName)
        {
            if (this._attributes == null)
            {
                return null;
            }
            return this.Attributes.GetAttribute(attributeShortName, attributeParentName);
        }

        /// <summary>
        /// Gets attribute with specified attribute short Name and parent name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Specifies Name of an attribute to search in current entity's attributes</param>
        /// <param name="attributeParentName">Specifies Parent Name of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Specifies Locale of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute in requested locale</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(String attributeShortName, String attributeParentName, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                return null;
            }
            return this.Attributes.GetAttribute(attributeShortName, attributeParentName, locale);
        }

        /// <summary>
        /// Gets attributes filtered by specific locale along with non localizable attributes.
        /// If locale is unknown than method doesn't apply filtering
        /// </summary>
        /// <param name="locale">Attribute locale to filter on</param>
        /// <returns>Attribute collection filtered by specific locale</returns>
        public IAttributeCollection GetAttributes(LocaleEnum locale)
        {
            IAttributeCollection filteredByLocaleAttributes = new AttributeCollection();

            if (this._attributes == null)
            {
                return null;
            }

            if (locale == LocaleEnum.UnKnown)
            {
                filteredByLocaleAttributes = this.Attributes;
            }
            else
            {
                foreach (Attribute attribute in this.Attributes)
                {
                    if (!attribute.IsLocalizable || (attribute.IsLocalizable && attribute.Locale == locale))
                    {
                        filteredByLocaleAttributes.Add(attribute);
                    }
                }
            }

            return filteredByLocaleAttributes;
        }

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttribute GetAttribute(Int32 attributeId)
        {
            if (this._attributes == null)
            {
                return null;
            }

            LocaleEnum locale = LocaleEnum.UnKnown;

            if (this.Locale != LocaleEnum.UnKnown)
            {
                Enum.TryParse<LocaleEnum>(this.Locale.ToString(), out locale);
            }

            return this.Attributes.GetAttribute(attributeId, locale);
        }

        /// <summary>
        /// Get attribute based on IAttributeUniqueIdentifier from current entity's attributes
        /// </summary>
        /// <param name="attributeUId">
        ///     IAttributeUniqueIdentifier which identifies an attribute uniquely
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeName is attribute short name
        ///     </para>
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeGroupName is attribute group short name
        ///     </para>
        /// </param>
        /// <returns>Attribute Interface</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="ArgumentException">AttributeUniqueIdentifier.AttributeGroupName and AttributeUniqueIdentifier.AttributeName is either null or empty</exception>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId)
        {
            if (this._attributes == null)
            {
                return null;
            }

            LocaleEnum locale = LocaleEnum.UnKnown;

            if (this.Locale != LocaleEnum.UnKnown)
            {
                Enum.TryParse<LocaleEnum>(this.Locale.ToString(), out locale);
            }

            return this.Attributes.GetAttribute(attributeUId, locale);
        }

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttribute GetAttribute(Int32 attributeId, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                return null;
            }

            return this.Attributes.GetAttribute(attributeId, locale);
        }

        /// <summary>
        /// Get attribute based on IAttributeUniqueIdentifier from current entity's attributes
        /// </summary>
        /// <param name="attributeUId">
        ///     IAttributeUniqueIdentifier which identifies an attribute uniquely
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeName is attribute short name
        ///     </para>
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeGroupName is attribute group short name
        ///     </para>
        /// </param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute Interface</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="ArgumentException">AttributeUniqueIdentifier.AttributeGroupName and AttributeUniqueIdentifier.AttributeName is either null or empty</exception>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                return null;
            }

            return this.Attributes.GetAttribute(attributeUId, locale);
        }

        /// <summary>
        /// Get common attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = Common</returns>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttributeCollection GetCommonAttributes()
        {
            if (this._attributes == null)
            {
                return null;
            }

            return this.Attributes.GetCommonAttributes();
        }

        /// <summary>
        /// Get Category specific attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = Category</returns>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttributeCollection GetCategorySpecificAttributes()
        {
            if (this._attributes == null)
            {
                return null;
            }

            return this.Attributes.GetCategorySpecificAttributes();
        }

        /// <summary>
        /// Get System attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = System </returns>
        /// <exception cref="NullReferenceException">Attributes for entity is null. There are no attributes to search in</exception>
        public IAttributeCollection GetSystemAttributes()
        {
            if (this._attributes == null)
            {
                return null;
            }

            return this.Attributes.GetSystemAttributes();
        }

        #endregion Attribute Get for current entity

        #region Attribute Value Get for current entity

        #region Current value

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName)
        {
            return this.Attributes.GetAttributeCurrentValueInstance(attributeName);
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies locale in which attribute value should returned </param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeCurrentValueInstance(attributeName, locale);
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName)
        {
            return this.Attributes.GetAttributeCurrentValueInstanceInvariant(attributeName);
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeCurrentValueInstanceInvariant(attributeName, locale);
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName)
        {
            return this.Attributes.GetAttributeCurrentValueInstance(attributeName, attributeParentName);
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeCurrentValueInstance(attributeName, attributeParentName, locale);
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName)
        {
            return this.Attributes.GetAttributeCurrentValueInstanceInvariant(attributeName, attributeParentName);
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeCurrentValueInstanceInvariant(attributeName, attributeParentName, locale);
        }

        #region Get Attribute Value as T type

        /// <summary>
        /// Returns the requested attribute's current value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current value as requested data type</returns>
        public T GetAttributeCurrentValue<T>(String attributeName)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValue<T>();
            }

            return attributeValue;
        }

        /// <summary>
        /// Returns the requested attribute's current value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current value as requested data type</returns>
        public T GetAttributeCurrentValue<T>(String attributeName, String attributeParentName)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName, attributeParentName);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValue<T>();
            }

            return attributeValue;
        }

        /// <summary>
        /// Returns the requested attribute's current value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates Locale in which value should be returned</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current value as requested data type</returns>
        public T GetAttributeCurrentValue<T>(String attributeName, LocaleEnum locale)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName, locale);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValue<T>();
            }

            return attributeValue;
        }

        /// <summary>
        /// Returns the requested attribute's current value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates Locale in which value should be returned</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current value as requested data type</returns>
        public T GetAttributeCurrentValue<T>(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName, attributeParentName, locale);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValue<T>();
            }

            return attributeValue;
        }

        /// <summary>
        /// Returns the requested attribute's current invariant value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current invariant value as requested data type</returns>
        public T GetAttributeCurrentValueInvariant<T>(String attributeName)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValueInvariant<T>();
            }

            return attributeValue;
        }

        /// <summary>
        /// Returns the requested attribute's current invariant value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current invariant value as requested data type</returns>
        public T GetAttributeCurrentValueInvariant<T>(String attributeName, String attributeParentName)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName, attributeParentName);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValueInvariant<T>();
            }

            return attributeValue;
        }

        /// <summary>
        /// Returns the requested attribute's current invariant value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="locale">Indicates Locale in which value should be returned</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current invariant value as requested data type</returns>
        public T GetAttributeCurrentValueInvariant<T>(String attributeName, LocaleEnum locale)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName, locale);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValueInvariant<T>();
            }

            return attributeValue;
        }

        /// <summary>
        /// Returns the requested attribute's current invariant value as requested data type 
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <param name="attributeParentName">Indicates the attribute parent short name</param>
        /// <param name="locale">Indicates Locale in which value should be returned</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the attribute's current invariant value as requested data type</returns>
        public T GetAttributeCurrentValueInvariant<T>(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            T attributeValue = default(T);

            var attribute = this.GetAttribute(attributeName, attributeParentName, locale);
            if (attribute != null)
            {
                attributeValue = attribute.GetCurrentValueInvariant<T>();
            }

            return attributeValue;
        }

        #endregion Get Attribute Value as T type

        #endregion

        #region Inherited Value

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName)
        {
            return this.Attributes.GetAttributeInheritedValueInstance(attributeName);
        }

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeInheritedValueInstance(attributeName, locale);
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName)
        {
            return this.Attributes.GetAttributeInheritedValueInstanceInvariant(attributeName);
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeInheritedValueInstanceInvariant(attributeName, locale);
        }

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName)
        {
            return this.Attributes.GetAttributeInheritedValueInstance(attributeName, attributeParentName);
        }

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeInheritedValueInstance(attributeName, attributeParentName, locale);
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName)
        {
            return this.Attributes.GetAttributeInheritedValueInstanceInvariant(attributeName, attributeParentName);
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            return this.Attributes.GetAttributeInheritedValueInstanceInvariant(attributeName, attributeParentName, locale);
        }

        #endregion

        #endregion

        #region Complex Attribute get utility methods

        #region Non collection attribute

        /// <summary>
        /// Gets the attribute as a complex attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttribute object</returns>
        public IComplexAttribute GetAttributeAsComplex(String attributeName, String parentName)
        {
            return GetComplexAttribute(attributeName, parentName);
        }

        /// <summary>
        /// Gets the attribute as a complex attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttribute object</returns>
        public IComplexAttribute GetAttributeAsComplex(String attributeName, String parentName, LocaleEnum localeFormat)
        {
            return GetComplexAttribute(attributeName, parentName, localeFormat);
        }

        /// <summary>
        /// Gets the attribute as complex attribute based on the attribute short name 
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttribute object</returns>
        public IComplexAttribute GetAttributeAsComplex(String attributeName)
        {
            return GetComplexAttribute(attributeName);
        }

        /// <summary>
        /// Gets the attribute as complex attribute based on the attribute short name 
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttribute object</returns>
        public IComplexAttribute GetAttributeAsComplex(String attributeName, LocaleEnum localeFormat)
        {
            return GetComplexAttribute(attributeName, localeFormat);
        }

        #endregion Non collection attribute

        #region Collection Attribute

        /// <summary>
        /// Gets the attribute as complex collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttributeCollection object</returns>
        public IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName, String parentName)
        {
            return GetComplexAttributeCollection(attributeName, parentName);
        }

        /// <summary>
        /// Gets the attribute as complex collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttributeCollection object</returns>
        public IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName, String parentName, LocaleEnum localeFormat)
        {
            return GetComplexAttributeCollection(attributeName, parentName, localeFormat);
        }

        /// <summary>
        /// Gets the attribute as complex collection attribute based on the attribute short name
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttributeCollection object</returns>
        public IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName)
        {
            return GetComplexAttributeCollection(attributeName);
        }

        /// <summary>
        /// Gets the attribute as complex collection attribute based on the attribute short name
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a complex attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttributeCollection object</returns>
        public IComplexAttributeCollection GetAttributeAsComplexCollection(String attributeName, LocaleEnum localeFormat)
        {
            return GetComplexAttributeCollection(attributeName, localeFormat);
        }

        #endregion Collection Attribute

        #endregion

        #region Collection Attribute get utility methods

        /// <summary>
        /// Gets the attribute as a collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ICollectionAttribute object</returns>
        public ICollectionAttribute GetAttributeAsCollection(String attributeName, String parentName)
        {
            return GetCollectionAttribute(attributeName, parentName);
        }

        /// <summary>
        /// Gets the attribute as a collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ICollectionAttribute object</returns>
        public ICollectionAttribute GetAttributeAsCollection(String attributeName, String parentName, LocaleEnum localeFormat)
        {
            return GetCollectionAttribute(attributeName, parentName, localeFormat);
        }

        /// <summary>
        /// Gets the attribute as collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ICollectionAttribute object</returns>
        public ICollectionAttribute GetAttributeAsCollection(String attributeName)
        {
            return GetCollectionAttribute(attributeName);
        }

        /// <summary>
        /// Gets the attribute as collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="ArgumentException">If attribute not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ICollectionAttribute object</returns>
        public ICollectionAttribute GetAttributeAsCollection(String attributeName, LocaleEnum localeFormat)
        {
            return GetCollectionAttribute(attributeName, localeFormat);
        }

        #endregion

        #region Lookup Attribute Get Utility Methods

        #region Non collection attribute

        /// <summary>
        /// Gets the attribute as a lookup attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IlookupAttribute object</returns>
        public ILookupAttribute GetAttributeAsLookup(String attributeName, String parentName)
        {
            return GetLookupAttribute(attributeName, parentName, this.Locale);
        }

        /// <summary>
        /// Gets the attribute as a lookup attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttribute object</returns>
        public ILookupAttribute GetAttributeAsLookup(String attributeName, String parentName, LocaleEnum localeFormat)
        {
            return GetLookupAttribute(attributeName, parentName, localeFormat);
        }

        /// <summary>
        /// Gets the attribute as lookup attribute based on the attribute short name 
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttribute object</returns>
        public ILookupAttribute GetAttributeAsLookup(String attributeName)
        {
            return GetLookupAttribute(attributeName, this.Locale);
        }

        /// <summary>
        /// Gets the attribute as lookup attribute based on the attribute short name 
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the IComplexAttribute object</returns>
        public ILookupAttribute GetAttributeAsLookup(String attributeName, LocaleEnum localeFormat)
        {
            return GetLookupAttribute(attributeName, localeFormat);
        }

        #endregion Non collection attribute

        #region Collection Attribute

        /// <summary>
        /// Gets the attribute as lookup collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ILookupCollectionAttribute object</returns>
        public ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName, String parentName)
        {
            return GetLookupAttributeCollection(attributeName, parentName, this.Locale);
        }

        /// <summary>
        /// Gets the attribute as lookup collection attribute based on the attribute short name and short name of the parent
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="parentName">Indicates the short name of the parent</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ILookupCollectionAttribute object</returns>
        public ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName, String parentName, LocaleEnum localeFormat)
        {
            return GetLookupAttributeCollection(attributeName, parentName, localeFormat);
        }

        /// <summary>
        /// Gets the attribute as lookup collection attribute based on the attribute short name
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ILookupCollectionAttribute object</returns>
        public ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName)
        {
            return GetLookupAttributeCollection(attributeName, this.Locale);
        }

        /// <summary>
        /// Gets the attribute as lookup collection attribute based on the attribute short name
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute</param>
        /// <param name="localeFormat">Indicates the attribute's locale</param>
        /// <exception cref="NotSupportedException">If attribute is not a lookup attribute</exception>
        /// <exception cref="NotSupportedException">If attribute is not a collection attribute</exception>
        /// <exception cref="ArgumentException">Attribute parent name and attribute Name is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <returns>Returns the ILookupCollectionAttribute object</returns>
        public ILookupCollectionAttribute GetAttributeAsLookupCollection(String attributeName, LocaleEnum localeFormat)
        {
            return GetLookupAttributeCollection(attributeName, localeFormat);
        }

        #endregion Collection Attribute

        #endregion

        #region Attribute Set

        /// <summary>
        /// Sets the attributes of the Entity
        /// </summary>
        /// <param name="iAttributeCollection">Attribute Collection Interface</param>
        public void SetAttributes(IAttributeCollection iAttributeCollection)
        {
            AttributeCollection attributeCollection = (AttributeCollection)iAttributeCollection;

            // Set current entity id to all attributes being added
            foreach (Attribute attr in attributeCollection)
                attr.EntityId = this.Id;

            this.Attributes = attributeCollection;
        }

        #endregion

        #region Set Attribute Value

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies Attribute Short Name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValue(String attributeName, IValue value)
        {
            IAttribute attribute = GetAttribute(attributeName);
            SetValue(attribute, value, false, this.Locale);
        }

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies Attribute Short Name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValue(String attributeName, Object value)
        {
            IAttribute attribute = GetAttribute(attributeName);
            SetValue(attribute, value, false, this.Locale);
        }

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValue(String attributeName, IValue value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, locale);
            SetValue(attribute, value, false, locale);
        }

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValue(String attributeName, Object value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, locale);
            SetValue(attribute, value, false, locale);
        }

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValue(String attributeName, String attributeParentName, IValue value)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName);
            SetValue(attribute, value, false, this.Locale);
        }

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValue(String attributeName, String attributeParentName, Object value)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName);
            SetValue(attribute, value, false, this.Locale);
        }

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValue(String attributeName, String attributeParentName, IValue value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName, locale);
            SetValue(attribute, value, false, locale);
        }

        /// <summary>
        /// Sets Attribute value for Current Entity. Overrides already existing value with new value.
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValue(String attributeName, String attributeParentName, Object value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName, locale);
            SetValue(attribute, value, false, locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValueInvariant(String attributeName, IValue value)
        {
            IAttribute attribute = GetAttribute(attributeName);
            SetValue(attribute, value, true, this.Locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValueInvariant(String attributeName, Object value)
        {
            IAttribute attribute = GetAttribute(attributeName);
            SetValue(attribute, value, true, this.Locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValueInvariant(String attributeName, IValue value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, locale);
            SetValue(attribute, value, true, locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValueInvariant(String attributeName, Object value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, locale);
            SetValue(attribute, value, true, locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValueInvariant(String attributeName, String attributeParentName, IValue value)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName);
            SetValue(attribute, value, true, this.Locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        public void SetAttributeValueInvariant(String attributeName, String attributeParentName, Object value)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName);
            SetValue(attribute, value, true, this.Locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValueInvariant(String attributeName, String attributeParentName, IValue value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName, locale);
            SetValue(attribute, value, true, locale);
        }

        /// <summary>
        /// Sets value as overridden
        /// </summary>
        /// <param name="attributeName">Specifies attribute short name</param>
        /// <param name="attributeParentName">Specifies attribute parent short name</param>
        /// <param name="value">Specifies Value Object to set</param>
        /// <param name="locale">Specifies Locale in which Attribute values should be set</param>
        public void SetAttributeValueInvariant(String attributeName, String attributeParentName, Object value, LocaleEnum locale)
        {
            IAttribute attribute = GetAttribute(attributeName, attributeParentName, locale);
            SetValue(attribute, value, true, locale);
        }

        #endregion Attribute Set

        #region Get entity context methods

        /// <summary>
        /// Get entity's context
        /// </summary>
        /// <returns>EntityContext interface</returns>
        /// <exception cref="NullReferenceException">Raised when EntityContext is null</exception>
        public IEntityContext GetContext()
        {
            if (this._entityContext == null)
            {
                return null;
            }

            return (IEntityContext)this.EntityContext;
        }

        /// <summary>
        /// Returns entity's Change Context
        /// </summary>
        /// <returns></returns>
        public IEntityChangeContext GetChangeContext(Boolean calculateLatest = false)
        {
            if (this._entityChangeContext == null || calculateLatest)
            {
                this._entityChangeContext = new EntityChangeContext(this);
            }

            return (IEntityChangeContext)this._entityChangeContext;
        }

        /// <summary>
        /// Returns entity's move context
        /// </summary>
        /// <returns>Returns EntityMoveContext interface</returns>
        /// <exception cref="NullReferenceException">Raised when EntityMoveContext is null</exception>
        public IEntityMoveContext GetEntityMoveContext()
        {
            if (this._entityMoveContext == null)
            {
                return null;
            }

            return (IEntityMoveContext)this.EntityMoveContext;
        }

        /// <summary>
        /// Returns related entity's change context
        /// </summary>
        /// <returns>Returns RelatedEntityChangeContext interface</returns>
        public IEntityChangeContext GetRelatedEntityChangeContext()
        {
            return this.RelatedEntityChangeContext;
        }

        #endregion

        #region Hierarchy Relationship Utility methods

        /// <summary>
        /// Gets hierarchy relationships
        /// </summary>
        /// <returns>Hierarchy Relationship collection interface</returns>
        public IHierarchyRelationshipCollection GetHierarchyRelationships()
        {
            return (IHierarchyRelationshipCollection)this.HierarchyRelationships;
        }

        /// <summary>
        /// Sets hierarchy relationships
        /// </summary>
        /// <param name="hierarchyRelationshipCollection">Hierarchy Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed hierarchy relationship collection is null</exception>
        public void SetHierarchyRelationships(IHierarchyRelationshipCollection hierarchyRelationshipCollection)
        {
            if (hierarchyRelationshipCollection == null)
                throw new ArgumentNullException("hierarchyRelationshipCollection");

            this.HierarchyRelationships = (HierarchyRelationshipCollection)hierarchyRelationshipCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntityCollection GetChildEntities()
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetChildEntities() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntity GetParentEntity()
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetParentEntity() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntityCollection GetAllParentEntities()
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetAllParentEntities() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntityCollection GetAllChildEntities()
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetAllChildEntities() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public IEntityCollection GetChildEntitiesByEntityTypeId(Int32 entityTypeId)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetChildEntitiesByEntityTypeId(entityTypeId) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        public IEntityCollection GetChildEntitiesByEntityTypeName(String entityTypeName)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetChildEntitiesByEntityTypeName(entityTypeName) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IEntity GetChildEntityByEntityId(Int64 entityId)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetChildEntityByEntityId(entityId) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public IEntity GetChildEntityByEntityName(String entityName)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetChildEntityByEntityName(entityName) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public IEntity GetParentEntityByEntityTypeId(Int32 entityTypeId)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetParentEntityByEntityTypeId(entityTypeId) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        public IEntity GetParentEntityByEntityTypeName(String entityTypeName)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetParentEntityByEntityTypeName(entityTypeName) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IEntity GetParentEntityByEntityId(Int64 entityId)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetParentEntityByEntityId(entityId) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public IEntity GetParentEntityByEntityName(String entityName)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetParentEntityByEntityName(entityName) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <param name="isRecursive"></param>
        public IEntity FindHierarchyRelatedEntity(Func<IHierarchyRelationship, Boolean> compareMethod, Boolean isRecursive = false)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.FindRelatedEntity(compareMethod, isRecursive) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <param name="isRecursive"></param>
        public IEntityCollection FindHierarchyRelatedEntities(Func<IHierarchyRelationship, Boolean> compareMethod, Boolean isRecursive = false)
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.FindRelatedEntities(compareMethod, isRecursive) : null;
        }

        /// <summary>
        /// Get all variants entities from current entity
        /// </summary>
        /// <returns></returns>
        private IEntityCollection GetAllVariantsEntities()
        {
            return _hierarchyRelationships != null ? _hierarchyRelationships.GetAllVariantsEntities() : null;
        }

        #endregion

        #region Extension Relationship Utility methods

        /// <summary>
        /// Gets the extended entity based on given container name and category path
        /// </summary>
        /// <param name="containerName">Indicates container name for which extended entity to be fetched</param>
        /// <param name="categoryPath">Indicates category path for which extended entity to be fetched</param>
        /// <returns>Returns extended entity if match found else null</returns>
        public IEntity GetExtendedEntity(String containerName, String categoryPath)
        {
            return (_extensionRelationships != null) ? _extensionRelationships.GetExtendedEntityByContainerNameAndCategoryPath(containerName, categoryPath) : null;
        }

        /// <summary>
        /// Gets all extended entities
        /// </summary>
        /// <returns>Returns all extended entities</returns>
        public IEntityCollection GetAllExtendedEntities()
        {
            return (_extensionRelationships != null) ? _extensionRelationships.GetAllExtendedEntities() : null;
        }

        /// <summary>
        /// Gets extension relationships
        /// </summary>
        /// <returns>Extension Relationship collection interface</returns>
        public IExtensionRelationshipCollection GetExtensionsRelationships()
        {
            return (IExtensionRelationshipCollection)this.ExtensionRelationships;
        }

        /// <summary>
        /// Sets extension relationships
        /// </summary>
        /// <param name="iExtensionRelationshipCollection">Extension Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed extension relationship collection is null</exception>
        public void SetExtensionRelationships(IExtensionRelationshipCollection iExtensionRelationshipCollection)
        {
            if (iExtensionRelationshipCollection == null)
                throw new ArgumentNullException("ExtensionRelationshipCollection");

            this.ExtensionRelationships = (ExtensionRelationshipCollection)iExtensionRelationshipCollection;
        }

        #endregion

        #region Relationship Utility methods

        /// <summary>
        /// Gets relationships
        /// </summary>
        /// <returns>Relationship collection interface</returns>
        public IRelationshipCollection GetRelationships()
        {
            IRelationshipCollection returnValue = null;
            if (this._relationships != null)
            {
                returnValue = (IRelationshipCollection)this.Relationships;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets relationships
        /// </summary>
        /// <param name="iRelationshipCollection">Relationship collection interface</param>
        /// <exception cref="ArgumentNullException">Raised when passed relationship collection is null</exception>
        public void SetRelationships(IRelationshipCollection iRelationshipCollection)
        {
            if (iRelationshipCollection == null)
                throw new ArgumentNullException("RelationshipCollection");

            this.Relationships = (RelationshipCollection)iRelationshipCollection;
        }

        #endregion Relationship Utility methods

        #region PermissionSet Methods

        /// <summary>
        /// Gets permission set for this entity in the current context
        /// </summary>
        /// <returns>Permission collection</returns>
        public Collection<UserAction> GetPermissionSet()
        {
            return this.PermissionSet;
        }

        #endregion

        #region AttributeModel Methods

        /// <summary>
        /// Sets the attributeModels of the Entity
        /// </summary>
        /// <param name="iAttributeModelCollection">AttributeModel Collection Interface</param>
        public void SetAttributeModels(IAttributeModelCollection iAttributeModelCollection)
        {
            AttributeModelCollection attributeModelCollection = (AttributeModelCollection)iAttributeModelCollection;

            this.AttributeModels = attributeModelCollection;
        }

        /// <summary>
        /// Gets the attributeModels belonging to the Entity
        /// </summary>
        /// <returns>AttributeModel Collection Interface</returns>
        /// <exception cref="NullReferenceException">AttributeModels for entity is null. There are no attributeModels to search in</exception>
        public IAttributeModelCollection GetAttributeModels()
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return (IAttributeModelCollection)this.AttributeModels;
        }

        /// <summary>
        /// Get All Common AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>Common AttributeModelCollection</returns>
        public IAttributeModelCollection GetCommonAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetCommonAttributeModels(locale);
        }

        /// <summary>
        /// Get All Required Common AttributeMdodels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredCommonAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }
            return this.AttributeModels.GetRequiredCommonAttributeModels(locale);
        }

        /// <summary>
        /// Get All Required and ReadOnly Common AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredReadOnlyCommonAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetRequiredReadOnlyCommonAttributeModels(locale);
        }

        /// <summary>
        /// Get All Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetTechnicalAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetTechnicalAttributeModels(locale);
        }

        /// <summary>
        /// Get All Required Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredTechnicalAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetRequiredTechnicalAttributeModels(locale);
        }

        /// <summary>
        /// Get All Required and ReadOnly Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredReadOnlyTechnicalAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetRequiredReadOnlyTechnicalAttributeModels(locale);
        }

        /// <summary>
        /// Get All Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRelationshipAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetRelationshipAttributeModels(locale);
        }

        /// <summary>
        /// Get All Required Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredRelationshipAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetRequiredRelationshipAttributeModels(locale);
        }

        /// <summary>
        /// Get All Required and ReadOnly Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredReadOnlyRelationshipAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetRequiredReadOnlyRelationshipAttributeModels(locale);
        }

        /// <summary>
        /// Gets Show At Creation Attribute Models
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetShowAtCreationAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetShowAtCreationAttributeModels(locale);
        }

        /// <summary>
        /// Gets AttributeModels which have default values configured
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetAttributeModelsWithDefaultValues(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                return null;
            }

            return this.AttributeModels.GetAttributeModelsWithDefaultValues(locale);
        }

        #endregion

        #region Entity Methods

        /// <summary>
        /// Create a new entity with basic properties of entity.
        /// </summary>
        /// <returns>New entity instance having same properties like current entity</returns>
        public Entity CloneBasicProperties()
        {
            Entity clonedEntity = new Entity();

            clonedEntity.Action = this.Action;
            clonedEntity.BranchLevel = this.BranchLevel;
            clonedEntity.CategoryId = this.CategoryId;
            clonedEntity.CategoryLongName = this.CategoryLongName;
            clonedEntity.CategoryName = this.CategoryName;
            clonedEntity.CategoryPath = this.CategoryPath;
            clonedEntity.CategoryLongNamePath = this.CategoryLongNamePath;
            clonedEntity.ContainerId = this.ContainerId;
            clonedEntity.ContainerLongName = this.ContainerLongName;
            clonedEntity.ContainerName = this.ContainerName;
            clonedEntity.EntityContext = this.EntityContext;
            clonedEntity.EntityTypeId = this.EntityTypeId;
            clonedEntity.EntityTypeLongName = this.EntityTypeLongName;
            clonedEntity.EntityTypeName = this.EntityTypeName;
            clonedEntity.EntityGuid = this.EntityGuid;
            clonedEntity.ExtensionUniqueId = this.ExtensionUniqueId;
            clonedEntity.ExternalId = this.ExternalId;
            clonedEntity.Id = this.Id;
            clonedEntity.IdPath = this.IdPath;
            clonedEntity.Locale = this.Locale;
            clonedEntity.LongName = this.LongName;
            clonedEntity.Name = this.Name;
            clonedEntity.OrganizationId = this.OrganizationId;
            clonedEntity.OrganizationLongName = this.OrganizationLongName;
            clonedEntity.OrganizationName = this.OrganizationName;
            clonedEntity.ParentEntityId = this.ParentEntityId;
            clonedEntity.ParentEntityLongName = this.ParentEntityLongName;
            clonedEntity.ParentEntityName = this.ParentEntityName;
            clonedEntity.ParentEntityTypeId = this.ParentEntityTypeId;
            clonedEntity.ParentExtensionEntityCategoryId = this.ParentExtensionEntityCategoryId;
            clonedEntity.ParentExtensionEntityCategoryLongName = this.ParentExtensionEntityCategoryLongName;
            clonedEntity.ParentExtensionEntityCategoryName = this.ParentExtensionEntityCategoryName;
            clonedEntity.ParentExtensionEntityCategoryPath = this.ParentExtensionEntityCategoryPath;
            clonedEntity.ParentExtensionEntityCategoryLongNamePath = this.ParentExtensionEntityCategoryLongNamePath;
            clonedEntity.ParentExtensionEntityContainerId = this.ParentExtensionEntityContainerId;
            clonedEntity.ParentExtensionEntityContainerLongName = this.ParentExtensionEntityContainerLongName;
            clonedEntity.ParentExtensionEntityContainerName = this.ParentExtensionEntityContainerName;
            clonedEntity.ParentExtensionEntityExternalId = this.ParentExtensionEntityExternalId;
            clonedEntity.ParentExtensionEntityId = this.ParentExtensionEntityId;
            clonedEntity.ParentExtensionEntityLongName = this.ParentExtensionEntityLongName;
            clonedEntity.ParentExtensionEntityName = this.ParentExtensionEntityName;
            clonedEntity.ParentExternalId = this.ParentExternalId;
            clonedEntity.Path = this.Path;
            clonedEntity.ProgramName = this.ProgramName;
            clonedEntity.ReferenceId = this.ReferenceId;
            clonedEntity.SequenceNumber = this.SequenceNumber;
            clonedEntity.ShowMaster = this.ShowMaster;
            clonedEntity.SKU = this.SKU;
            clonedEntity.EntityFamilyId = this.EntityFamilyId;
            clonedEntity.EntityFamilyLongName = this.EntityFamilyLongName;
            clonedEntity.EntityGlobalFamilyId = this.EntityGlobalFamilyId;
            clonedEntity.EntityGlobalFamilyLongName = this.EntityGlobalFamilyLongName;
            clonedEntity.HierarchyLevel = this.HierarchyLevel;
            clonedEntity.CrossReferenceGuid = this.CrossReferenceGuid;
            clonedEntity.CrossReferenceId = this.CrossReferenceId;

            if (this._sourceInfo != null)
            {
                clonedEntity._sourceInfo = (SourceInfo)this._sourceInfo.Clone();
            }
            else
            {
                clonedEntity._sourceInfo = null;
            }

            return clonedEntity;
        }

        /// <summary>
        /// Clone Entity object
        /// </summary>
        /// <returns>Returns cloned copy of entity</returns>
        public Entity Clone()
        {
            Entity clonedEntity = CloneBasicProperties();

            #region Clone Permission Set

            if (this._permissionSet != null && this._permissionSet.Count > 0)
            {
                clonedEntity._permissionSet = ValueTypeHelper.CloneCollection(this._permissionSet);
            }

            #endregion Clone Permission Set

            if (this._attributes != null)
            {
                clonedEntity.Attributes = (AttributeCollection)this.Attributes.Clone();
            }

            if (this._attributeModels != null)
            {
                clonedEntity.AttributeModels = this.AttributeModels.Clone();
            }

            if (this._entityContext != null)
            {
                clonedEntity.EntityContext = (EntityContext)this.EntityContext.Clone();
            }

            if (this._relationships != null)
            {
                clonedEntity.Relationships = this.Relationships.Clone();
            }

            if (this._extensionRelationships != null)
            {
                clonedEntity.ExtensionRelationships = this.ExtensionRelationships.Clone();
            }

            if (this._hierarchyRelationships != null)
            {
                clonedEntity.HierarchyRelationships = this.HierarchyRelationships.Clone();
            }

            if (this._workflowStates != null)
            {
                clonedEntity.WorkflowStates = this.WorkflowStates.Clone();
            }

            if (this._workflowActionContext != null)
            {
                clonedEntity.WorkflowActionContext = this.WorkflowActionContext.Clone();
            }

            return clonedEntity;
        }

        /// <summary>
        /// Get the Original Entity
        /// </summary>
        /// <returns>IEntity</returns>
        public IEntity GetOriginalEntity()
        {
            IEntity returnValue = null;

            if (this.OriginalEntity != null)
            {
                returnValue = (IEntity)this.OriginalEntity;
            }
            return returnValue;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeCreateAction"></param>
        /// <returns></returns>
        public Boolean HasChanged(Boolean includeCreateAction = false)
        {
            if (includeCreateAction)
                return this.Action == ObjectAction.Create
                || this.Action == ObjectAction.Update
                || this.Action == ObjectAction.ReParent
                || this.Action == ObjectAction.Reclassify
                || this.Action == ObjectAction.Rename
                || this.Action == ObjectAction.Delete;
            else
                return this.Action == ObjectAction.Update
                    || this.Action == ObjectAction.ReParent
                    || this.Action == ObjectAction.Reclassify
                    || this.Action == ObjectAction.Rename
                    || this.Action == ObjectAction.Delete;
        }

        /// <summary>
        /// Marks the entity object properties (AttributeModels and/or Relationships) as ReadOnly 
        /// If AttributeModels is set to ReadOnly then HierarchyRelationships and ExtensionRelationships are also set to ReadOnly
        /// </summary>
        /// <param name="markAttributeModelsAsReadOnly">Indicates whether to mark attribute models as readonly or not</param>
        /// <param name="markRelationshipsAsReadOnly">Indicates whether to mark relationships as readonly or not</param>
        public void MarkAsReadOnly(Boolean markAttributeModelsAsReadOnly = true, Boolean markRelationshipsAsReadOnly = true)
        {
            if (markAttributeModelsAsReadOnly && this.AttributeModels != null)
            {
                this.AttributeModels.MarkAsReadOnly();
            }

            if (this._relationships != null && markRelationshipsAsReadOnly)
            {
                this.Relationships.MarkAsReadOnly();
            }
        }

        /// <summary>
        /// Gets all flatten entities of entire family including variants and extensions including variants
        /// </summary>
        /// <param name="includeSelf">Flag indicates to include self entity or not</param>
        /// <param name="includeVariants">Flag indicates to include self entity or not</param>
        /// <param name="includeExtensionsWithVariants">Flag indicates to include self entity or not</param>
        /// <returns>Returns all flatten entities of entire family including variants and extensions including variants</returns>
        public IEntityCollection GetFlattenEntities(Boolean includeSelf = true, Boolean includeVariants = true, Boolean includeExtensionsWithVariants = true)
        {
            EntityCollection allEntities = new EntityCollection();

            if (includeSelf)
            {
                allEntities.Add(this);
            }

            #region Get all variants entities

            if (includeVariants)
            {
                EntityCollection allVariantsEntities = this.GetAllVariantsEntities() as EntityCollection;

                if (allVariantsEntities != null && allVariantsEntities.Count > 0)
                {
                    allEntities.AddRange(allVariantsEntities);
                }
            }

            #endregion Get all variants entities

            #region Get all extensions entities including variants

            if (includeExtensionsWithVariants)
            {
                EntityCollection allExtendedEntities = this.GetAllExtendedEntities() as EntityCollection;

                if (allExtendedEntities != null && allExtendedEntities.Count > 0)
                {
                    foreach (Entity extendedEntity in allExtendedEntities)
                    {
                        allEntities.Add(extendedEntity);

                        EntityCollection allExtendedEntitiesVariants = extendedEntity.GetAllVariantsEntities() as EntityCollection;

                        if (allExtendedEntitiesVariants != null && allExtendedEntitiesVariants.Count > 0)
                        {
                            allEntities.AddRange(allExtendedEntitiesVariants);
                        }
                    }
                }
            }

            #endregion Get all extensions entities including variants

            return allEntities;
        }

        /// <summary>
        /// Gets entity type name to variants level map from entity object
        /// </summary>
        /// <returns>Returns entity type name to variants level map from entity object for the variants tree</returns>
        public Dictionary<Int32, String> GetEntityVariantsLevel()
        {
            Dictionary<Int32, String> entityTypeNameToVariantsLevelMaps = null;

            EntityCollection entities = (EntityCollection)this.GetFlattenEntities(true, true);

            if (entities != null && entities.Count > 0)
            {
                entityTypeNameToVariantsLevelMaps = new Dictionary<Int32, String>();

                foreach (Entity entity in entities)
                {
                    String entityTypeName = entity.EntityTypeName;

                    if (!entityTypeNameToVariantsLevelMaps.ContainsKey(entity.HierarchyLevel))
                    {
                        entityTypeNameToVariantsLevelMaps.Add(entity.HierarchyLevel, entityTypeName);
                    }
                }
            }

            return entityTypeNameToVariantsLevelMaps;
        }

        #endregion

        #region Workflow Information Methods

        /// <summary>
        /// Gets Entity's current workflow Information.
        /// </summary>
        /// <returns>TrackedActivityInfo Collection Interface</returns>
        /// <exception cref="NullReferenceException">TrackedActivityInfoCollection for entity is null. There are no Tracked workflow Activity Information to search in</exception>
        public IWorkflowStateCollection GetWorkflowDetails()
        {
            if (this._workflowStates == null)
            {
                return null;
            }
            return (WorkflowStateCollection)this.WorkflowStates;
        }

        /// <summary>
        /// Gets Entity's current workflow action context.
        /// </summary>
        /// <returns>Workflow Action Context Interface</returns>
        public IWorkflowActionContext GetWorkflowActionContext()
        {
            return (WorkflowActionContext)this.WorkflowActionContext;
        }

        /// <summary>
        /// Sets workflow action context
        /// </summary>
        /// <param name="iWorkflowActionContext">Workflow Action Context interface</param>
        public void SetWorkflowActionContext(IWorkflowActionContext iWorkflowActionContext)
        {
            this.WorkflowActionContext = (WorkflowActionContext)iWorkflowActionContext;
        }

        #endregion

        #region Replace Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <param name="newAttributeInstance"></param>
        /// <returns></returns>
        public Boolean ReplaceAttribute(Int32 attributeId, LocaleEnum locale, IAttribute newAttributeInstance)
        {
            this._attributes.Replace(attributeId, locale, newAttributeInstance);
            return true;
        }

        #endregion

        #region validation state

        /// <summary>
        /// Gets the validation state belonging to the Entity
        /// </summary>
        /// <returns>validation state Interface</returns> 
        public IValidationStates GetValidationStates()
        {
            if (this._validationStates == null)
            {
                return _validationStates;
            }

            return (IValidationStates)this._validationStates;
        }

        /// <summary>
        /// Sets the validation state.
        /// </summary>
        /// <param name="iValidationStates">The validation state.</param>
        public void SetValidationStates(IValidationStates iValidationStates)
        {
            this._validationStates = (ValidationStates)iValidationStates;
        }

        #endregion

        #region Get BusinessConditionStatusCollection
        
        /// <summary>
        /// Gets IBusinessConditionStatusCollection for current entity
        /// </summary>
        /// <returns>Returns IBusinessConditionStatusCollection</returns>
        public IBusinessConditionStatusCollection GetBusinessConditionStatusCollection()
        {
            return (IBusinessConditionStatusCollection)this.BusinessConditions;
        }

        #endregion Get BusinessConditionStatusCollection

        #endregion

        #region Private Method

        /// <summary>
        /// Load entity object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value
        /// <![CDATA[
        /// 	<Entity 
        ///     	Id="4384979" 
        ///     	ObjectId="320207" 
        ///     	Name="P2001" 
        ///     	LName="P2001" 
        ///     	SKU="P2001" 
        ///     	Path="Bluestem 
        ///     	Brands#@#Product Master#@#!   ! GrandParent Category#@#P2001" 
        ///     	CatPath = "!   ! GrandParent Category#@#P2001"
        ///     	BranchLevel="1" 
        ///     	ParentEnId="4384912" 
        ///     	ParentEnName="!   ! GrandParent Category" 
        ///     	ParentEnLName="!   ! GrandParent Category" 
        ///     	CatId="4384912" 
        ///     	CatName="!   ! GrandParent Category" 
        ///     	CatLName="!   ! GrandParent Category" 
        ///     	ContId="17" 
        ///         ContName="Product Master" 
        ///         ContLName="Product Master" 
        ///         OrgId="4" 
        ///         OrgName="Bluestem Brands" 
        ///         OrgLName="!   ! GrandParent Category" 
        ///         EnTypeId="7" 
        ///         EnTypeName="4" 
        ///         EnTypeLName="Product" 
        ///         Locale="en_WW" 
        ///         LocaleId="1"
        ///         Action="Read" 
        ///         EffectiveFrom="" 
        ///         EffectiveTo="" 
        ///         EntityLastModTime="" 
        ///         EntityLastModUser="">
        ///         	<Attributes>
        ///             	<Attribute Id="3099" Name="Product ID" LName="Product ID" ValRefId="120" AttrParentId="3128" AttrParentName="Core Attributes" AttrParentLName="Core Attributes" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttrType="Simple" AttrModelType="Common" AttrDataType="String" SourceFlag="O" Action="Update">
        ///                 	<Values SourceFlag="O">
        ///                     	<Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW">P2001</Value>
        ///                     </Values>
        ///                         <Values SourceFlag="I" />
        ///                 </Attribute >
        ///             </Attributes>
        ///         </Entity>
        /// 	]]>   
        /// </param>
        private void LoadEntityDetailForDataTransfer(String valuesAsXml)
        {
            #region Sample Xml
            /*
                Id="4384979" 
              	ObjectId="320207" 
             	Name="P2001" 
            	LName="P2001" 
            	SKU="P2001" 
               	Path="Bluestem 
            	Brands#@#Product Master#@#!   ! GrandParent Category#@#P2001" 
            	CatPath = "!   ! GrandParent Category#@#P2001"
            	BranchLevel="1" 
            	ParentEnId="4384912" 
            	ParentEnName="!   ! GrandParent Category" 
            	ParentEnLName="!   ! GrandParent Category" 
            	CatId="4384912" 
             	CatName="!   ! GrandParent Category" 
            	CatLName="!   ! GrandParent Category" 
               	ContId="17" 
                ContName="Product Master" 
                ContLName="Product Master" 
                OrgId="4" 
                OrgName="Bluestem Brands" 
                OrgLName="!   ! GrandParent Category" 
                EnTypeId="7" 
                EnTypeName="4" 
                EnTypeLName="Product" 
                Locale="en_WW" 
                LocaleId="1"
                Action="Read" 
                EffectiveFrom="" 
                EffectiveTo="" 
                EntityLastModTime="" 
                EntityLastModUser="">
                	<Attributes>
                    	<Attribute Id="3099" Name="Product ID" LName="Product ID" ValRefId="120" AttrParentId="3128" AttrParentName="Core Attributes" AttrParentLName="Core Attributes" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttrType="Simple" AttrModelType="Common" AttrDataType="String" SourceFlag="O" Action="Update">
                        	<Values SourceFlag="O">
                            	<Value Uom="" ValRefId="-1" Seq="0" Locale="en_WW">P2001</Value>
                           </Values>
                                <Values SourceFlag="I" />
                        </Attribute >
                    </Attributes>
               </Entity>
               */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Entity")
                        {
                            //Read entity metadata
                            #region Read Entity Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("N"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LN"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("A"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("CatLNP"))
                                {
                                    this.CategoryLongNamePath = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PEId"))
                                {
                                    this.ParentEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ParentEntityId);
                                }

                                if (reader.MoveToAttribute("PETId"))
                                {
                                    this.ParentEntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ParentEntityTypeId);
                                }

                                if (reader.MoveToAttribute("PELN"))
                                {
                                    this.ParentEntityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PExEId"))
                                {
                                    this.ParentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.ParentExtensionEntityId);
                                }

                                if (reader.MoveToAttribute("PExELN"))
                                {
                                    this.ParentExtensionEntityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CatId"))
                                {
                                    this.CategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.CategoryId);
                                }

                                if (reader.MoveToAttribute("CatLN"))
                                {
                                    this.CategoryLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ContainerId);
                                }

                                if (reader.MoveToAttribute("ContLN"))
                                {
                                    this.ContainerLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OId"))
                                {
                                    this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.OrganizationId);
                                }

                                if (reader.MoveToAttribute("OLN"))
                                {
                                    this.OrganizationLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ETId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.EntityTypeId);
                                }

                                if (reader.MoveToAttribute("ETLN"))
                                {
                                    this.EntityTypeLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("L"))
                                {
                                    String strLocale = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                                    if (locale == LocaleEnum.UnKnown)
                                        throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");

                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("PS"))
                                {
                                    this.PermissionSet = Utility.GetPermissionsAsObject(reader.ReadContentAsString());
                                }

                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
                            //Read attributes
                            #region Read attributes

                            String attributeXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(attributeXml))
                            {
                                AttributeCollection attributeCollection = new AttributeCollection(attributeXml, false, ObjectSerialization.DataTransfer);

                                if (attributeCollection != null)
                                {
                                    // Based on the serialization type the unique 
                                    foreach (Attribute attr in attributeCollection)
                                    {
                                        if (!this.Attributes.Contains(attr.Id, attr.Locale))
                                            this.Attributes.Add(attr);
                                    }
                                }
                            }

                            #endregion Read attributes
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                        {
                            // Read relationships
                            #region Read Relationships

                            String relationshipXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(relationshipXml))
                            {
                                RelationshipCollection relationshipCollection = new RelationshipCollection(relationshipXml, ObjectSerialization.DataTransfer);

                                if (relationshipCollection != null)
                                {
                                    this.Relationships.PermissionSet = relationshipCollection.PermissionSet;
                                    this.Relationships.IsDenormalizedRelationshipsDirty = relationshipCollection.IsDenormalizedRelationshipsDirty;

                                    foreach (Relationship relationship in relationshipCollection)
                                    {
                                        if (this._relationships == null || !this.Relationships.Contains(relationship))
                                        {
                                            this.Relationships.Add(relationship);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Hierarchies")
                        {
                            // Read Hierarchy Relationships
                            #region Read Hierarchy Relationships

                            String hierarchyRelationshipXml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(hierarchyRelationshipXml))
                            {
                                HierarchyRelationshipCollection HierarchyRelationshipCollection = new HierarchyRelationshipCollection(hierarchyRelationshipXml, ObjectSerialization.DataTransfer);
                                if (HierarchyRelationshipCollection != null)
                                {
                                    foreach (HierarchyRelationship hierarchyRelationship in HierarchyRelationshipCollection)
                                    {
                                        if (!this.HierarchyRelationships.Contains(hierarchyRelationship))
                                        {
                                            this.HierarchyRelationships.Add(hierarchyRelationship);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Extensions")
                        {
                            // Read Extension Relationships
                            #region Read Extension Relationships

                            String extensionRelationshipXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(extensionRelationshipXml))
                            {
                                ExtensionRelationshipCollection extensionrelationshipCollection = new ExtensionRelationshipCollection(extensionRelationshipXml, ObjectSerialization.DataTransfer);
                                if (extensionrelationshipCollection != null && extensionrelationshipCollection.Count > 0)
                                {
                                    foreach (ExtensionRelationship extensionrelationship in extensionrelationshipCollection)
                                    {
                                        if (!this.ExtensionRelationships.Contains(extensionrelationship))
                                        {
                                            this.ExtensionRelationships.Add(extensionrelationship);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowStates")
                        {
                            // Read Workflow Information's
                            #region Read Workflow Information's

                            String workflowInformationsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(workflowInformationsXml))
                            {
                                WorkflowStateCollection wfStateCollection = new WorkflowStateCollection(workflowInformationsXml, ObjectSerialization.DataTransfer);
                                if (wfStateCollection != null)
                                {
                                    this.WorkflowStates = wfStateCollection;
                                }
                            }

                            #endregion
                        }
                        //else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowActionContext")
                        //{
                        //    // Read Workflow action context Information's
                        //    #region Read Workflow Information's

                        //    String workflowActionContextXml = reader.ReadOuterXml();

                        //    if (!String.IsNullOrWhiteSpace(workflowActionContextXml))
                        //    {
                        //        WorkflowActionContext wfActionContext = new WorkflowActionContext(workflowActionContextXml, ObjectSerialization.DataTransfer);
                        //        if (wfActionContext != null)
                        //        {
                        //            this.WorkflowActionContext = wfActionContext;
                        //        }
                        //    }

                        //    #endregion
                        //}
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }

                    #region Update Entity.EntityContext with DataLocales from Attribute in Entity



                    if (this._attributes != null)
                    {
                        //Get distinct list of locales available in entity's attributes.
                        Collection<LocaleEnum> locales = this.Attributes.GetLocaleList();
                        if (locales != null && this.EntityContext.DataLocales.Count < 1)
                        {
                            foreach (LocaleEnum locale in locales)
                            {
                                this.EntityContext.DataLocales.Add(locale);
                            }
                        }
                    }

                    #endregion Update Entity.EntityContext with DataLocales from Attribute in Entity
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Loads entity meta-data from reader
        /// </summary>
        /// <param name="reader">Indicates an xml reader generated from xml representation of Entity object</param>
        private void LoadEntityMetadataFromXml(XmlTextReader reader)
        {
            #region Read Entity Attributes

            if (reader.MoveToAttribute("Id"))
            {
                this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.Id);
            }

            if (reader.MoveToAttribute("ExtensionUniqueId"))
            {
                this._extensionUniqueId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._extensionUniqueId);
            }

            if (reader.MoveToAttribute("ExternalId"))
            {
                this._externalId = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ReferenceId"))
            {
                this._referenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._referenceId);
            }

            if (reader.MoveToAttribute("ParentExternalId"))
            {
                this._parentExternalId = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Name"))
            {
                this.Name = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LongName"))
            {
                this.LongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Action"))
            {
                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
            }

            if (reader.MoveToAttribute("SKU"))
            {
                this._sku = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("Path"))
            {
                this._path = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("CategoryPath"))
            {
                this._categoryPath = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("CategoryLongNamePath"))
            {
                this._categoryLongNamePath = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("TargetCategoryPath"))   //This will be used only for RsXml to support reclassification.
            {
                this.EntityMoveContext.TargetCategoryPath = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("BranchLevel"))
            {
                this._branchLevel = (ContainerBranchLevel)reader.ReadContentAsInt();
            }

            if (reader.MoveToAttribute("ParentEntityId"))
            {
                this._parentEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._parentEntityId);
            }

            if (reader.MoveToAttribute("ParentEntityTypeId"))
            {
                this._parentEntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._parentEntityTypeId);
            }

            if (reader.MoveToAttribute("ParentEntityName"))
            {
                this._parentEntityName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentEntityLongName"))
            {
                this._parentEntityLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityId"))
            {
                this._parentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._parentExtensionEntityId);
            }

            if (reader.MoveToAttribute("ParentExtensionEntityName"))
            {
                this._parentExtensionEntityName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityLongName"))
            {
                this._parentExtensionEntityLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityExternalId"))
            {
                this._parentExtensionEntityExternalId = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityContainerId"))
            {
                this._parentExtensionEntityContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._parentExtensionEntityContainerId);
            }

            if (reader.MoveToAttribute("ParentExtensionEntityContainerName"))
            {
                this._parentExtensionEntityContainerName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityContainerLongName"))
            {
                this._parentExtensionEntityContainerLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityCategoryId"))
            {
                this._parentExtensionEntityCategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._parentExtensionEntityCategoryId);
            }

            if (reader.MoveToAttribute("ParentExtensionEntityCategoryName"))
            {
                this._parentExtensionEntityCategoryName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityCategoryLongName"))
            {
                this._parentExtensionEntityCategoryLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityCategoryPath"))
            {
                this._parentExtensionEntityCategoryPath = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ParentExtensionEntityCategoryLongNamePath"))
            {
                this._parentExtensionEntityCategoryLongNamePath = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("CategoryId"))
            {
                this._categoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._categoryId);
            }

            if (reader.MoveToAttribute("CategoryName"))
            {
                this._categoryName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("CategoryLongName"))
            {
                this._categoryLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ContainerId"))
            {
                this._containerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
            }

            if (reader.MoveToAttribute("ContainerName"))
            {
                this._containerName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ContainerLongName"))
            {
                this._containerLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("OrganizationId"))
            {
                this._organizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._organizationId);
            }

            if (reader.MoveToAttribute("OrganizationName"))
            {
                this._organizationName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("OrganizationLongName"))
            {
                this._organizationLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("EntityTypeId"))
            {
                this._entityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._entityTypeId);
            }

            if (reader.MoveToAttribute("EntityTypeName"))
            {
                this._entityTypeName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("EntityTypeLongName"))
            {
                this._entityTypeLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("LocaleId"))
            {
                Int32 localeId = reader.ReadContentAsInt();
                LocaleEnum locale = LocaleEnum.UnKnown;
                locale = (LocaleEnum)localeId;

                if (locale == LocaleEnum.UnKnown)
                {
                    throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");
                }

                this.Locale = locale;
            }
            else if (reader.MoveToAttribute("Locale"))
            {
                String strLocale = reader.ReadContentAsString();
                LocaleEnum locale = LocaleEnum.UnKnown;
                Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                if (locale == LocaleEnum.UnKnown)
                {
                    throw new ArgumentOutOfRangeException("Locale Id is out of range. Please verify provided locale and try again.");
                }

                this.Locale = locale;
            }

            if (reader.MoveToAttribute("SequenceNumber"))
            {
                this._sequenceNumber = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._sequenceNumber);
            }

            if (reader.MoveToAttribute("SourceClass"))
            {
                this._sourceClass = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
            }

            if (reader.MoveToAttribute("IsDirectChange"))
            {
                this._isDirectChange = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), true);
            }

            if (reader.MoveToAttribute("EntityTreeIdList"))
            {
                this._entityTreeIdList = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ExtensionUniqueId"))
            {
                this._extensionUniqueId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._extensionUniqueId);
            }

            if (reader.MoveToAttribute("EntityFamilyId"))
            {
                this._entityFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityFamilyId);
            }

            if (reader.MoveToAttribute("EntityFamilyLongName"))
            {
                this._entityFamilyLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("EntityGlobalFamilyId"))
            {
                this._entityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityGlobalFamilyId);
            }

            if (reader.MoveToAttribute("EntityGlobalFamilyLongName"))
            {
                this._entityGlobalFamilyLongName = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("IdPath"))
            {
                this._idPath = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("SourceId"))
            {
                SourceInfo sourceInfo = new SourceInfo();
                sourceInfo.SourceId = reader.ReadContentAsInt();
                if (reader.MoveToAttribute("SourceEntityId"))
                {
                    sourceInfo.SourceEntityId = reader.ReadContentAsLong();
                }
                this._sourceInfo = sourceInfo;
            }

            if (reader.MoveToAttribute("EntityGuid"))
            {
                String entityUniqueId = reader.ReadContentAsString();

                if (!String.IsNullOrEmpty(entityUniqueId))
                {
                    this._entityGuid = new Guid(entityUniqueId);
                }
            }

            if (reader.MoveToAttribute("CrossReferenceId"))
            {
                this._crossReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._crossReferenceId);
            }

            if (reader.MoveToAttribute("CrossReferenceGuid"))
            {
                String entityCrossReferenceGuid = reader.ReadContentAsString();

                if (!String.IsNullOrEmpty(entityCrossReferenceGuid))
                {
                    this._crossReferenceGuid = new Guid(entityCrossReferenceGuid);
                }
            }

            #endregion Read Entity Attributes
        }

        #region Complex Attribute Helpers

        private IComplexAttribute GetComplexAttribute(String attributeName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            IComplexAttribute complexAttribute = null;
            IAttribute attribute = GetParentComplexAttribute(attributeName, localeFormat);

            if (attribute != null)
            {
                if (!attribute.IsCollection)
                {
                    complexAttribute = new ComplexAttribute(attribute);
                }
                else
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} is a complex collection attribute. Use GetAttributeAsComplexCollection() method for getting complex collection attribute", attributeName);
                    throw new NotSupportedException(exceptionMessage);
                }
            }
            return complexAttribute;
        }

        private IComplexAttributeCollection GetComplexAttributeCollection(String attributeName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            IComplexAttributeCollection complexAttribute = null;
            IAttribute attribute = GetParentComplexAttribute(attributeName, localeFormat);

            if (attribute != null)
            {
                if (attribute.IsCollection)
                {
                    complexAttribute = new ComplexAttributeCollection(attribute);
                }
                else
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} is not a complex collection attribute.Use GetAttributeAsComplex() method for getting complex attribute", attributeName);
                    throw new NotSupportedException(exceptionMessage);
                }
            }
            return complexAttribute;
        }

        private IAttribute GetParentComplexAttribute(String attributeName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            IAttribute attribute = GetAttribute(attributeName, localeFormat != LocaleEnum.UnKnown ? localeFormat : this.Locale);

            if (attribute != null && !attribute.IsComplex)
            {
                String exceptionMessage = String.Format("Attribute with name: {0} is not a complex attribute", attributeName);
                throw new NotSupportedException(exceptionMessage);
            }

            return attribute;
        }

        private IComplexAttribute GetComplexAttribute(String attributeName, String parentName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            IComplexAttribute complexAttribute = null;
            IAttribute attribute = GetParentComplexAttribute(attributeName, parentName, localeFormat);

            if (attribute != null)
            {
                if (!attribute.IsCollection)
                {
                    complexAttribute = new ComplexAttribute(attribute);
                }
                else
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} and group name: {1} is a complex collection attribute", attributeName, parentName);
                    throw new NotSupportedException(exceptionMessage);
                }
            }
            return complexAttribute;
        }

        private IComplexAttributeCollection GetComplexAttributeCollection(String attributeName, String parentName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            IComplexAttributeCollection complexAttribute = null;
            IAttribute attribute = GetParentComplexAttribute(attributeName, parentName, localeFormat);

            if (attribute != null)
            {
                if (attribute.IsCollection)
                {
                    complexAttribute = new ComplexAttributeCollection(attribute);
                }
                else
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} and group name: {1} is not a complex collection attribute", attributeName, parentName);
                    throw new NotSupportedException(exceptionMessage);
                }
            }
            return complexAttribute;
        }

        private IAttribute GetParentComplexAttribute(String attributeName, String parentName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            IAttribute attribute = GetAttribute(new AttributeUniqueIdentifier(attributeName, parentName), localeFormat != LocaleEnum.UnKnown ? localeFormat : this.Locale);

            if (attribute != null && !attribute.IsComplex)
            {
                String exceptionMessage = String.Format("Attribute with name: {0} and group name: {1} is not a complex attribute", attributeName, parentName);
                throw new NotSupportedException(exceptionMessage);
            }

            return attribute;
        }

        #endregion Complex Attribute Helpers

        #region Lookup Attribute Helpers

        private ILookupAttribute GetLookupAttribute(String attributeName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            ILookupAttribute lookupAttribute = null;
            IAttribute attribute = GetAttribute(attributeName, localeFormat);

            if (attribute != null)
            {
                VerifyIfLookupAttribute(attribute, false);
                lookupAttribute = new LookupAttribute(attribute);
            }

            return lookupAttribute;
        }

        private ILookupCollectionAttribute GetLookupAttributeCollection(String attributeName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            ILookupCollectionAttribute lookupAttributeCollection = null;
            IAttribute attribute = GetAttribute(attributeName, localeFormat);

            if (attribute != null)
            {
                VerifyIfLookupAttribute(attribute, true);
                lookupAttributeCollection = new LookupCollectionAttribute(attribute);
            }

            return lookupAttributeCollection;
        }

        private ILookupAttribute GetLookupAttribute(String attributeName, String attributeParentName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            ILookupAttribute lookupAttribute = null;
            IAttribute attribute = GetAttribute(attributeName, attributeParentName, localeFormat);

            if (attribute != null)
            {
                VerifyIfLookupAttribute(attribute, false);
                lookupAttribute = new LookupAttribute(attribute);
            }

            return lookupAttribute;
        }

        private ILookupCollectionAttribute GetLookupAttributeCollection(String attributeName, String attributeParentName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            ILookupCollectionAttribute lookupAttributeCollection = null;
            IAttribute attribute = GetAttribute(attributeName, attributeParentName, localeFormat);

            if (attribute != null)
            {
                VerifyIfLookupAttribute(attribute, true);
                lookupAttributeCollection = new LookupCollectionAttribute(attribute);
            }

            return lookupAttributeCollection;
        }

        private void VerifyIfLookupAttribute(IAttribute attribute, Boolean verifyCollectionAttribute)
        {
            String errorMessage = String.Empty;

            if (!attribute.IsLookup)
            {
                errorMessage = String.Format("Attribute with name: {0} and group name: {1} is not a lookup attribute",
                    attribute.Name, attribute.AttributeParentName);
            }
            else if (verifyCollectionAttribute && !attribute.IsCollection)
            {
                errorMessage = String.Format("Attribute with name: {0} and group name: {1} is not a lookup collection attribute.Use GetAttributeAsLookup() method for getting lookup attribute",
                    attribute.Name, attribute.AttributeParentName);
            }
            else if (!verifyCollectionAttribute && attribute.IsCollection)
            {
                errorMessage = String.Format("Attribute with name: {0} and group name: {1} is a lookup collection attribute. Use GetAttributeAsLookupCollection() method for getting lookup collection attribute",
                    attribute.Name, attribute.AttributeParentName);
            }

            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                throw new NotSupportedException(errorMessage);
            }
        }

        #endregion

        private ICollectionAttribute GetCollectionAttribute(String attributeName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            Attribute attribute = (Attribute)GetAttribute(attributeName, localeFormat == LocaleEnum.UnKnown ? this.Locale : localeFormat);
            ICollectionAttribute collectionAttribute = null;

            if (attribute != null)
            {
                if (attribute.IsComplex && attribute.IsCollection)
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} is a complex collection attribute. Please call GetAttributeAsComplex", attributeName);
                    throw new NotSupportedException(exceptionMessage);
                }
                else if (!attribute.IsCollection)
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} is not a collection attribute", attributeName);
                    throw new NotSupportedException(exceptionMessage);
                }
                else
                {
                    collectionAttribute = (ICollectionAttribute)new CollectionAttribute(attribute);
                }
            }

            return collectionAttribute;
        }

        private ICollectionAttribute GetCollectionAttribute(String attributeName, String attributeParentName, LocaleEnum localeFormat = LocaleEnum.UnKnown)
        {
            Attribute attribute = (Attribute)GetAttribute(attributeName, attributeParentName, localeFormat == LocaleEnum.UnKnown ? this.Locale : localeFormat);
            ICollectionAttribute collectionAttribute = null;

            if (attribute != null)
            {
                if (attribute.IsComplex && attribute.IsCollection)
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} and group name: {1} is a complex collection attribute. Please call GetAttributeAsComplex", attributeName, attributeParentName);
                    throw new NotSupportedException(exceptionMessage);
                }
                else if (!attribute.IsCollection)
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} and group name: {1} is not a collection attribute", attributeName, attributeParentName);
                    throw new NotSupportedException(exceptionMessage);
                }
                else
                {
                    collectionAttribute = (ICollectionAttribute)new CollectionAttribute(attribute);
                }
            }

            return collectionAttribute;
        }

        private void SetValue(IAttribute attribute, IValue value, Boolean setInvariant, LocaleEnum locale)
        {
            if (attribute != null)
            {
                if (attribute.SourceFlag == AttributeValueSource.Inherited)
                {
                    attribute.SourceFlag = AttributeValueSource.Overridden;
                }

                if (setInvariant)
                {
                    attribute.SetValueInvariant(value);
                }
                else
                {
                    attribute.SetValue(value, locale);
                }
            }
            else
            {
                String exceptionDetails = String.Format("Requested Attribute is found Null.Failed to set value in Entity : {0} {1}", this.Id, this.Name);
                throw new ArgumentNullException(exceptionDetails);
            }
        }

        private void SetValue(IAttribute attribute, Object valueObj, Boolean setInvariant, LocaleEnum locale)
        {
            Value value = new Value(valueObj);
            value.Action = ObjectAction.Update;
            SetValue(attribute, value, setInvariant, locale);
        }

        /// <summary>
        /// Converts properties (meta-data) of Entity object into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml representation of Entity meta-data</param>
        private void ConvertEntityMetadataToXml(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId);
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("SKU", this.SKU);
            xmlWriter.WriteAttributeString("Path", this.Path);
            xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath);
            xmlWriter.WriteAttributeString("CategoryLongNamePath", this.CategoryLongNamePath);
            xmlWriter.WriteAttributeString("BranchLevel", ((Int32)this.BranchLevel).ToString());
            xmlWriter.WriteAttributeString("ParentEntityId", this.ParentEntityId.ToString());
            xmlWriter.WriteAttributeString("ParentEntityTypeId", this.ParentEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ParentExternalId", this.ParentExternalId);
            xmlWriter.WriteAttributeString("ParentEntityName", this.ParentEntityName);
            xmlWriter.WriteAttributeString("ParentEntityLongName", this.ParentEntityLongName);
            xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
            xmlWriter.WriteAttributeString("ParentExtensionEntityName", this.ParentExtensionEntityName);
            xmlWriter.WriteAttributeString("ParentExtensionEntityLongName", this.ParentExtensionEntityLongName);
            xmlWriter.WriteAttributeString("ParentExtensionEntityExternalId", this.ParentExtensionEntityExternalId);
            xmlWriter.WriteAttributeString("ParentExtensionEntityContainerId", this.ParentExtensionEntityContainerId.ToString());
            xmlWriter.WriteAttributeString("ParentExtensionEntityContainerName", this.ParentExtensionEntityContainerName);
            xmlWriter.WriteAttributeString("ParentExtensionEntityContainerLongName", this.ParentExtensionEntityContainerLongName);
            xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryId", this.ParentExtensionEntityCategoryId.ToString());
            xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryName", this.ParentExtensionEntityCategoryName);
            xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryLongName", this.ParentExtensionEntityCategoryLongName);
            xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryPath", this.ParentExtensionEntityCategoryPath);
            xmlWriter.WriteAttributeString("ParentExtensionEntityCategoryLongNamePath", this.ParentExtensionEntityCategoryLongNamePath);
            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("CategoryName", this.CategoryName);
            xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName);
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
            xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName);
            xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
            xmlWriter.WriteAttributeString("OrganizationName", this.OrganizationName);
            xmlWriter.WriteAttributeString("OrganizationLongName", this.OrganizationLongName);
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
            xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("SequenceNumber", this.SequenceNumber.ToString());
            xmlWriter.WriteAttributeString("SourceClass", this.SourceClass.ToString());
            xmlWriter.WriteAttributeString("EffectiveFrom", "");
            xmlWriter.WriteAttributeString("EffectiveTo", "");
            xmlWriter.WriteAttributeString("EntityLastModTime", "");
            xmlWriter.WriteAttributeString("EntityLastModUser", "");
            xmlWriter.WriteAttributeString("IsDirectChange", this.IsDirectChange.ToString());
            xmlWriter.WriteAttributeString("EntityTreeIdList", this.EntityTreeIdList);
            xmlWriter.WriteAttributeString("ExtensionUniqueId", this.ExtensionUniqueId.ToString());
            xmlWriter.WriteAttributeString("IdPath", this.IdPath);
            xmlWriter.WriteAttributeString("EntityGuid", this.EntityGuid == null ? String.Empty : this.EntityGuid.ToString());
            xmlWriter.WriteAttributeString("EntityFamilyId", this.EntityFamilyId.ToString());
            xmlWriter.WriteAttributeString("EntityFamilyLongName", this.EntityFamilyLongName);
            xmlWriter.WriteAttributeString("EntityGlobalFamilyId", this.EntityGlobalFamilyId.ToString());
            xmlWriter.WriteAttributeString("EntityGlobalFamilyLongName", this.EntityGlobalFamilyLongName.ToString());
            xmlWriter.WriteAttributeString("CrossReferenceId", this.CrossReferenceId.ToString());
            xmlWriter.WriteAttributeString("CrossReferenceGuid", this.CrossReferenceGuid == null ? String.Empty : this.CrossReferenceGuid.ToString());
        }

        /// <summary>
        /// Converts SourceInfo object into xml
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml representation of SourceInfo object</param>
        private void ConvertSourceInfoToXml(XmlTextWriter xmlWriter)
        {
            if (this.SourceInfo != null)
            {
                xmlWriter.WriteAttributeString("SourceId", this.SourceInfo.SourceId.ToString());
                xmlWriter.WriteAttributeString("SourceEntityId", this.SourceInfo.SourceEntityId.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlWriter"></param>
        /// <param name="entityStateValidations"></param>
        /// <param name="systemAttribute"></param>
        private void PopulateEntityStateValidationBasedOnSystemAttribute(XmlTextWriter xmlWriter, EntityStateValidationCollection entityStateValidations, SystemAttributes systemAttribute)
        {
            if (entityStateValidations != null)
            {
                EntityStateValidationCollection systemAttributeStateValidation = entityStateValidations.Get(MV => MV.SystemValidationStateAttribute == systemAttribute);
                Collection<String> messageCodes = new Collection<String>();

                if (systemAttributeStateValidation != null && systemAttributeStateValidation.Count > 0)
                {
                    xmlWriter.WriteRaw(systemAttributeStateValidation.ToXml(ObjectSerialization.External));
                }
            }
        }


        #endregion

        #endregion
    }
}