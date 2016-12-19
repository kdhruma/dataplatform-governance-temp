using ProtoBuf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies EntityContext which indicates what all information is to be loaded in Entity object
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(EntityModelContext))]
    [KnownType(typeof(AttributeContext))]
    [KnownType(typeof(RelationshipContext))]
    [KnownType(typeof(EntityHierarchyContext))]
    public class EntityContext : ObjectBase, IEntityContext, IAttributeContext, IEntityDataContext
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private EntityModelContext _entityModelContext = new EntityModelContext();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private AttributeContext _attributeContext = new AttributeContext();

        /// <summary>
        /// Field denoting the relationship context
        /// </summary>
        [DataMember(Order = 0)]
        [ProtoMember(3)]
        private RelationshipContext _relationshipContext = null;

        /// <summary>
        /// Field denoting the EntityHierarchy Context 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        private EntityHierarchyContext _entityHierarchyContext = null;

        /// <summary>
        /// Field denoting the EntityExtension Context 
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        private EntityExtensionContext _entityExtensionContext = null;

        /// <summary>
        /// Field denoting
        /// </summary>
        [DataMember]
        [ProtoMember(6), DefaultValue(LocaleEnum.UnKnown)]
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting Locale collection for Entity's data locale
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        private Collection<LocaleEnum> _dataLocales = new Collection<LocaleEnum>() { LocaleEnum.UnKnown };

        /// <summary>
        /// Field denoting whether to load entity properties or not. Here entity properties refers to entity metadata like short name, long name, parent information
        /// </summary>
        [DataMember]
        [ProtoMember(8), DefaultValue(true)]
        private Boolean _loadEntityProperties = true;

        /// <summary>
        /// Field denoting whether to load attributes or not.
        /// IF this flag is set to false then even if AttributeIds and group ids are given, no attributes will be loaded.
        /// So if any attributes are required to be fetched then this flag needs to be set to true.
        /// </summary>
        [DataMember]
        [ProtoMember(9), DefaultValue(false)]
        private Boolean _loadAttributes = false;

        /// <summary>
        /// Property denoting whether to load relationships or not.
        /// If LoadRelationships = true and no value is given in Relationship Context's RelationshipTypeIdList then all relationships of mapped relationship type will be loaded
        /// If LoadRelationships = true and there are some relationship type ids given in Relationship Context's RelationshipTypeIdList then relationships of given relationship type(s) will be loaded
        /// </summary>
        [DataMember]
        [ProtoMember(10), DefaultValue(false)]
        private Boolean _loadRelationships = false;

        /// <summary>
        /// Field denoting whether to load hierarchy relationships or not
        /// </summary>
        [DataMember]
        [ProtoMember(11), DefaultValue(false)]
        private Boolean _loadHierarchyRelationships = false;

        /// <summary>
        /// Field denoting whether to load extension relationships or not
        /// </summary>
        [DataMember]
        [ProtoMember(12), DefaultValue(false)]
        private Boolean _loadExtensionRelationships = false;

        /// <summary>
        /// Field denoting whether to load sources or not.
        /// </summary>
        [DataMember]
        [ProtoMember(13), DefaultValue(false)]
        private Boolean _loadSources = false;

        /// <summary>
        /// Field denoting whether to load Workflow Information or not
        /// </summary>
        [DataMember]
        [ProtoMember(14), DefaultValue(false)]
        private Boolean _loadWorkflowInformation = false;

        /// <summary>
        /// Field denoting the workflowName
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        private String _workflowName = String.Empty;

        /// <summary>
        /// Field denoting whether to ignore entity status or not while getting entity details
        /// When set to true, Entity Get will return Entity Even if it has been deleted.
        /// </summary>
        [DataMember]
        [ProtoMember(16), DefaultValue(false)]
        private Boolean _ignoreEntityStatus = false;

        /// <summary>
        /// Property denoting whether to fill missing ids by names or not
        /// </summary>
        [DataMember]
        [ProtoMember(17), DefaultValue(false)]
        private Boolean _resolveIdsByNames = false;

        /// <summary>
        /// Field used for storing extra information for entityContext
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        private String _extendedProperties = String.Empty;

        /// <summary>
        /// Field denoting whether to load business conditions or not
        /// </summary>
        [DataMember]
        [ProtoMember(19)]
        private Boolean _loadBusinessConditions = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityContext()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes container id, categoryId, entityTypeId, parentEntityId, locale, loadEntityProperties, loadAttributes,
        /// attributesGroupIdList, loadCreationAttributes, loadRequiredAttributes, and attributeModelType as input parameters
        /// </summary>
        /// <param name="containerId">Indicates the identity of container of an entity</param>
        /// <param name="categoryId">Indicates the identity of category of an entity</param>
        /// <param name="entityTypeId">Indicates the identity of entityType</param>
        /// <param name="locale">Indicates the enum of locale</param>
        /// <param name="dataLocales"> Indicates the enum of current data locale</param>
        /// <param name="loadEntityProperties">Indicates whether to load the entityProperties or not</param>
        /// <param name="loadAttributes">Indicates whether to load attributes or not</param>
        /// <param name="loadHierarchyRelationships">Indicates whether to load hierarchical relationship or not</param>
        /// <param name="loadExtensionRelationship">Indicates whether to load extension relationship or not</param>
        /// <param name="attributeGroupIdList">Indicates the collection of identity of attributeGroup</param>
        /// <param name="attributeIdList">Indicates the collection of identity of attribute</param>
        /// <param name="loadRelationships">Indicates whether to load relationship or not</param>
        /// <param name="loadRelationshipAttributes">Indicates whether to load relationship attributes or not</param>
        /// <param name="relationshipTypeIdList">Indicates the collection of identity of relationship type</param>
        /// <param name="loadCreationAttributes">Indicate whether to load creation attribute or not</param>
        /// <param name="loadRequiredAttributes">Indicates whether to load required attributes or not</param>
        /// <param name="attributeModelType">Indicates the enums of attributeModelType whose entities to be loaded</param>
        public EntityContext(Int32 containerId, Int64 categoryId, Int32 entityTypeId, LocaleEnum locale, Collection<LocaleEnum> dataLocales,
            Boolean loadEntityProperties, Boolean loadAttributes, Boolean loadHierarchyRelationships, Boolean loadExtensionRelationship, Collection<Int32> attributeGroupIdList, Collection<Int32> attributeIdList,
            Boolean loadRelationships, Boolean loadRelationshipAttributes, Collection<Int32> relationshipTypeIdList,
            Boolean loadCreationAttributes, Boolean loadRequiredAttributes, AttributeModelType attributeModelType)
        {
            this.InitialiseFields(containerId, categoryId, entityTypeId, locale, dataLocales, loadEntityProperties, loadAttributes,
                        loadHierarchyRelationships, loadExtensionRelationship, attributeGroupIdList, attributeIdList, loadRelationships, loadRelationshipAttributes, relationshipTypeIdList,
                        loadCreationAttributes, loadRequiredAttributes, attributeModelType);
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityContext(String valuesAsXml)
        {
            LoadEntityContext(valuesAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public EntityContext(Entity entity)
        {
            EntityContext entityContext = this;
            Collection<Int32> attributeIdList = new Collection<Int32>();
            Collection<LocaleEnum> dataLocales = new Collection<LocaleEnum>();

            foreach (Attribute attribute in entity.Attributes)
            {
                if (!attributeIdList.Contains(attribute.Id))
                {
                    attributeIdList.Add(attribute.Id);
                }

                if (!dataLocales.Contains(attribute.Locale))
                {
                    dataLocales.Add(attribute.Locale);
                }
            }

            entityContext.ContainerId = entity.ContainerId;
            entityContext.EntityTypeId = entity.EntityTypeId;
            entityContext.CategoryId = entity.CategoryId;
            entityContext.Locale = entity.Locale;

            entityContext.LoadAttributeModels = false;
            entityContext.LoadWorkflowInformation = false;

            if (attributeIdList != null && attributeIdList.Count > 0)
            {
                entityContext.AttributeIdList = attributeIdList;
                entityContext.LoadAttributes = true;
            }

            if (!dataLocales.Contains(entity.Locale))
            {
                dataLocales.Add(entity.Locale);
            }

            entityContext.DataLocales = dataLocales;

            var relationships = entity.GetRelationships() as RelationshipCollection;

            if (relationships != null && relationships.Count > 0)
            {
                //Enable load relationships and relationship attributes 
                entityContext.LoadRelationships = true;
                entityContext.RelationshipContext.LoadRelationshipAttributes = true;
                entityContext.RelationshipContext.DataLocales = entityContext.DataLocales;

                //Populate distinct list of locales and relationship type id available in each of relationship attributes.
                PopulateRelationshipTypeIdListAndLocales(relationships, entityContext.RelationshipContext);
            }

            if (entity.HierarchyRelationships != null && entity.HierarchyRelationships.Count > 0)
            {
                // Enable load hierarchy relationships
                entityContext.LoadHierarchyRelationships = true;
            }

            if (entity.ExtensionRelationships != null && entity.ExtensionRelationships.Count > 0)
            {
                // Enable load extension relationship
                entityContext.LoadExtensionRelationships = true;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denoting
        /// </summary>
        public LocaleEnum Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        /// <summary>
        /// Property denoting Locale collection for entity attribute value.
        /// </summary>
        public Collection<LocaleEnum> DataLocales
        {
            get
            {
                var dataLocales = new Collection<LocaleEnum>();

                if (_dataLocales != null && _dataLocales.Count > 0)
                {
                    foreach (var locale in _dataLocales)
                    {
                        if (locale != LocaleEnum.UnKnown && !dataLocales.Contains(locale))
                        {
                            dataLocales.Add(locale);
                        }
                    }
                }

                _dataLocales = dataLocales;

                return _dataLocales;
            }
            set
            {
                _dataLocales = value;
            }
        }

        /// <summary>
        /// Property denoting container id to which entity belongs
        /// </summary>
        public Int32 ContainerId
        {
            get
            {
                return _entityModelContext.ContainerId;
            }
            set
            {
                _entityModelContext.ContainerId = value;
            }
        }

        /// <summary>
        /// Property denoting short name of the container
        /// </summary>
        public String ContainerName
        {
            set { _entityModelContext.ContainerName = value; }
            get { return _entityModelContext.ContainerName; }
        }

        /// <summary>
        /// Property denoting id of container qualifier
        /// </summary>
        public Int32 ContainerQualifierId
        {
            get
            {
                return _entityModelContext.ContainerQualifierId;
            }
            set
            {
                _entityModelContext.ContainerQualifierId = value;
            }
        }

        /// <summary>
        /// Property denoting name of container qualifier
        /// </summary>
        public String ContainerQualifierName
        {
            get
            {
                return _entityModelContext.ContainerQualifierName;
            }
            set
            {
                _entityModelContext.ContainerQualifierName = value;
            }
        }

        /// <summary>
        /// Property denoting category id to which entity belongs
        /// </summary>
        public Int64 CategoryId
        {
            get
            {
                return _entityModelContext.CategoryId;
            }
            set
            {
                _entityModelContext.CategoryId = value;
            }
        }

        /// <summary>
        /// Property denoting short name of the category
        /// </summary>
        public String CategoryPath
        {
            set { _entityModelContext.CategoryPath = value; }
            get { return _entityModelContext.CategoryPath; }
        }

        /// <summary>
        /// Property denoting entity type id of current entity
        /// </summary>
        public Int32 EntityTypeId
        {
            get
            {
                return _entityModelContext.EntityTypeId;
            }
            set
            {
                _entityModelContext.EntityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting short name of the entity type
        /// </summary>
        public String EntityTypeName
        {
            set { _entityModelContext.EntityTypeName = value; }
            get { return _entityModelContext.EntityTypeName; }
        }

        /// <summary>
        /// Property denoting state view id.
        /// </summary>
        public Int32 StateViewId
        {
            get
            {
                return _attributeContext.StateViewId;
            }
            set
            {
                _attributeContext.StateViewId = value;
            }
        }

        /// <summary>
        /// Property denoting state view name.
        /// </summary>
        public String StateViewName
        {
            get
            {
                return _attributeContext.StateViewName;
            }
            set
            {
                _attributeContext.StateViewName = value;
            }
        }

        /// <summary>
        /// Property denoting custom view id.
        /// </summary>
        public Int32 CustomViewId
        {
            get
            {
                return _attributeContext.CustomViewId;
            }
            set
            {
                _attributeContext.CustomViewId = value;
            }
        }

        /// <summary>
        /// Property denoting custom view name.
        /// </summary>
        public String CustomViewName
        {
            get
            {
                return _attributeContext.CustomViewName;
            }
            set
            {
                _attributeContext.CustomViewName = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load entity properties or not. Here entity properties refers to entity metadata like short name, long name, parent information
        /// </summary>
        public Boolean LoadEntityProperties
        {
            get
            {
                return _loadEntityProperties;
            }
            set
            {
                _loadEntityProperties = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load attributes or not.
        /// IF this flag is set to false then even if AttributeIds and group ids are given, no attributes will be loaded.
        /// So if any attributes are required to be fetched then this flag needs to be set to true.
        /// </summary>
        public Boolean LoadAttributes
        {
            get
            {
                return _loadAttributes;
            }
            set
            {
                _loadAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load only current attribute values (Either Inherited or Overridden based on availability) or not.
        /// </summary>
        public Boolean LoadOnlyCurrentValues
        {
            get
            {
                return _attributeContext.LoadOnlyCurrentValues;
            }
            set
            {
                _attributeContext.LoadOnlyCurrentValues = value;
            }
        }

        /// <summary>
        /// Field denoting whether to load only Inherited attributes or not.
        /// </summary>
        public Boolean LoadOnlyInheritedValues
        {
            get
            {
                return _attributeContext.LoadOnlyInheritedValues;
            }
            set
            {
                _attributeContext.LoadOnlyInheritedValues = value;
            }
        }

        /// <summary>
        /// Field denotes whether to load inheritable only attributes or not.
        /// </summary>
        public Boolean LoadInheritableOnlyAttributes
        {
            get
            {
                return _attributeContext.LoadInheritableOnlyAttributes;
            }
            set
            {
                _attributeContext.LoadInheritableOnlyAttributes = value;
            }
        }

        /// <summary>
        /// Field denoting whether to load only Overridden attributes or not.
        /// </summary>
        public Boolean LoadOnlyOverridenValues
        {
            get
            {
                return _attributeContext.LoadOnlyOverridenValues;
            }
            set
            {
                _attributeContext.LoadOnlyOverridenValues = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load hierarchy relationships or not
        /// </summary>
        public Boolean LoadHierarchyRelationships
        {
            get
            {
                return _loadHierarchyRelationships;
            }
            set
            {
                _loadHierarchyRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load extension relationships or not
        /// </summary>
        public Boolean LoadExtensionRelationships
        {
            get
            {
                return _loadExtensionRelationships;
            }
            set
            {
                _loadExtensionRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting attributes belonging to which group are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificGroupIdList.
        /// To load attributes from group ids given in AttributeGroupIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        public Collection<Int32> AttributeGroupIdList
        {
            get
            {
                return _attributeContext.AttributeGroupIdList;
            }
            set
            {
                _attributeContext.AttributeGroupIdList = value;
            }
        }

        /// <summary>
        /// Property denoting which attributes are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificAttributeIdList.
        /// To load attributes from attribute ids given in AttributeIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        public Collection<Int32> AttributeIdList
        {
            get
            {
                return _attributeContext.AttributeIdList;
            }
            set
            {
                _attributeContext.AttributeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as ShowAtCreation
        /// </summary>
        public Boolean LoadCreationAttributes
        {
            get
            {
                return _attributeContext.LoadCreationAttributes;
            }
            set
            {
                _attributeContext.LoadCreationAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as Required
        /// </summary>
        public Boolean LoadRequiredAttributes
        {
            get
            {
                return _attributeContext.LoadRequiredAttributes;
            }
            set
            {
                _attributeContext.LoadRequiredAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load relationships or not.
        /// If LoadRelationships = true and no value is given in Relationship Context's RelationshipTypeIdList then all relationships of mapped relationship type will be loaded
        /// If LoadRelationships = true and there are some relationship type ids given in Relationship Context's RelationshipTypeIdList then relationships of given relationship type(s) will be loaded
        /// </summary>
        public Boolean LoadRelationships
        {
            get
            {
                return _loadRelationships;
            }
            set
            {
                _loadRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load sources or not.
        /// </summary>
        public Boolean LoadSources
        {
            get
            {
                return _loadSources;
            }
            set
            {
                _loadSources = value;
            }
        }

        /// <summary>
        /// Property denoting which type of attributes are to be loaded. Possible values <see cref="AttributeModelType"/>
        /// </summary>
        public AttributeModelType AttributeModelType
        {
            get
            {
                return _attributeContext.AttributeModelType;
            }
            set
            {
                _attributeContext.AttributeModelType = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load AttributeModel or not.
        /// </summary>
        public Boolean LoadAttributeModels
        {
            get
            {
                return _attributeContext.LoadAttributeModels;
            }
            set
            {
                this._attributeContext.LoadAttributeModels = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load workflow Information or not.
        /// </summary>
        public Boolean LoadWorkflowInformation
        {
            get
            {
                return _loadWorkflowInformation;
            }
            set
            {
                this._loadWorkflowInformation = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Name
        /// </summary>
        public String WorkflowName
        {
            get
            {
                return _workflowName;
            }
            set
            {
                this._workflowName = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load lookup display values
        /// </summary>
        public Boolean LoadLookupDisplayValues
        {
            get
            {
                return _attributeContext.LoadLookupDisplayValues;
            }
            set
            {
                _attributeContext.LoadLookupDisplayValues = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load LookupRow along with Value or not
        /// </summary>
        public Boolean LoadLookupRowWithValues
        {
            get
            {
                return _attributeContext.LoadLookupRowWithValues;
            }
            set
            {
                _attributeContext.LoadLookupRowWithValues = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load dependent attribute or not
        /// </summary>
        public Boolean LoadDependentAttributes
        {
            get
            {
                return _attributeContext.LoadDependentAttributes;
            }
            set
            {
                this._attributeContext.LoadDependentAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether to ignore entity status or not while getting entity details
        /// When set to true, Entity Get will return Entity Even if it has been deleted.
        /// </summary>
        public Boolean IgnoreEntityStatus
        {
            get
            {
                return this._ignoreEntityStatus;
            }
            set
            {
                this._ignoreEntityStatus = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load attributes having blank values / no value
        /// When set to true, Entity Get will return attribute object instances having blank / no value
        /// </summary>
        public Boolean LoadBlankAttributes
        {
            get
            {
                return _attributeContext.LoadBlankAttributes;
            }
            set
            {
                this._attributeContext.LoadBlankAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting the relationship context
        /// </summary>
        public RelationshipContext RelationshipContext
        {
            get
            {
                if (this._relationshipContext == null)
                {
                    this._relationshipContext = new RelationshipContext();
                }

                return _relationshipContext;
            }
            set
            {
                _relationshipContext = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean LoadComplexChildAttributes
        {
            get
            {
                return _attributeContext.LoadComplexChildAttributes;
            }
            set
            {
                _attributeContext.LoadComplexChildAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting shortname of the attribute with Group Name
        /// </summary>
        public Collection<String> AttributeNames
        {
            get
            {
                return _attributeContext.AttributeNames;
            }
            set
            {
                _attributeContext.AttributeNames = value;
            }
        }

        /// <summary>
        /// Property denoting short name of the attribute group name with Parent Name
        /// </summary>
        public Collection<String> AttributeGroupNames
        {
            get
            {
                return _attributeContext.AttributeGroupNames;
            }
            set
            {
                _attributeContext.AttributeGroupNames = value;
            }
        }

        /// <summary>
        /// Property denoting whether to fill missing ids by names or not
        /// </summary>
        public Boolean ResolveIdsByName
        {
            get
            {
                return this._resolveIdsByNames;
            }
            set
            {
                this._resolveIdsByNames = value;
            }
        }

        /// <summary>
        /// Property specifying if the attribute loading is required after considering related options in the context
        /// </summary>
        public Boolean IsAttributeLoadingRequired
        {
            get
            {
                return (this.LoadAttributes || this.LoadOnlyInheritedValues || this.LoadOnlyOverridenValues || this.LoadOnlyCurrentValues || this.LoadStateValidationAttributes);
            }
        }

        /// <summary>
        /// Property denoting the Entity Hierarchy context
        /// </summary>
        public EntityHierarchyContext EntityHierarchyContext
        {
            get
            {
                if (_entityHierarchyContext == null)
                {
                    _entityHierarchyContext = new EntityHierarchyContext();
                }

                return _entityHierarchyContext;
            }
            set
            {
                _entityHierarchyContext = value;
            }
        }
        /// <summary>
        /// Specifies whether to load state validation attributes or not.
        /// </summary>
        public Boolean LoadStateValidationAttributes
        {
            get { return _attributeContext.LoadStateValidationAttributes; }
            set { _attributeContext.LoadStateValidationAttributes = value; }
        }

        /// <summary>
        /// Property used for storing extra information for entityContext
        /// </summary>
        public String ExtendedProperties
        {
            get
            {
                return this._extendedProperties;
            }
            set
            {
                this._extendedProperties = value;
            }
        }

        /// <summary>
        /// Property denoting the Entity Extension context
        /// </summary>
        public EntityExtensionContext EntityExtensionContext
        {
            get
            {
                if (_entityExtensionContext == null)
                {
                    _entityExtensionContext = new EntityExtensionContext();
                }

                return _entityExtensionContext;
            }
            set
            {
                _entityExtensionContext = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load business conditions or not
        /// </summary>
        public Boolean LoadBusinessConditions
        {
            get { return _loadBusinessConditions; }
            set { _loadBusinessConditions = value; }
        }
        
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents EntityContext  in Xml format
        /// </summary>
        /// <returns>String representation of current EntityContext object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            String attributeIds = String.Empty;
            String attributeGroupIds = String.Empty;
            String relationshipTypeIds = String.Empty;
            String dataLocales = String.Empty;
            String requestedRelationshipsList = String.Empty;

            if (this.AttributeIdList != null)
            {
                attributeIds = ValueTypeHelper.JoinCollection(this.AttributeIdList, ",");
            }

            if (this.AttributeGroupIdList != null)
            {
                attributeGroupIds = ValueTypeHelper.JoinCollection(this.AttributeGroupIdList, ",");
            }

            if (this._relationshipContext != null && this._relationshipContext.RelationshipTypeIdList != null)
            {
                relationshipTypeIds = ValueTypeHelper.JoinCollection(this._relationshipContext.RelationshipTypeIdList, ",");
            }

            if (this.DataLocales != null)
            {
                dataLocales = ValueTypeHelper.JoinCollection(this.DataLocales, ",");
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            // xmlWriter.WriteStartDocument();

            // Attribute node start
            xmlWriter.WriteStartElement("EntityContext");

            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerQualifierId", this.ContainerQualifierId.ToString());
            xmlWriter.WriteAttributeString("ContainerQualifierName", this.ContainerQualifierName);
            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("DataLocales", dataLocales);
            xmlWriter.WriteAttributeString("StateViewId", this.StateViewId.ToString());
            xmlWriter.WriteAttributeString("StateViewName", this.StateViewName);
            xmlWriter.WriteAttributeString("CustomViewId", this.CustomViewId.ToString());
            xmlWriter.WriteAttributeString("CustomViewName", this.CustomViewName);
            xmlWriter.WriteAttributeString("LoadEntityProperties", this.LoadEntityProperties.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadAttributes", this.LoadAttributes.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadOnlyCurrentValues", this.LoadOnlyCurrentValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadInheritableOnlyAttributes", this.LoadInheritableOnlyAttributes.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadHierarchyRelationships", this.LoadHierarchyRelationships.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadExtensionRelationships", this.LoadExtensionRelationships.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("AttributeGroupIdList", attributeGroupIds);
            xmlWriter.WriteAttributeString("AttributeIdList", attributeIds);
            xmlWriter.WriteAttributeString("LoadRelationships", this.LoadRelationships.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadSources", this.LoadSources.ToString().ToLowerInvariant());

            xmlWriter.WriteAttributeString("LoadRelationshipAttributes", this._relationshipContext != null ? this._relationshipContext.LoadRelationshipAttributes.ToString().ToLowerInvariant() : "false");
            xmlWriter.WriteAttributeString("RelationshipTypeIdList", relationshipTypeIds);
            xmlWriter.WriteAttributeString("RelationshipLevelsToBeConsidered", this._relationshipContext != null ? this._relationshipContext.Level.ToString() : "0");
            xmlWriter.WriteAttributeString("LoadCreationAttributes", this.LoadCreationAttributes.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadRequiredAttributes", this.LoadRequiredAttributes.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
            xmlWriter.WriteAttributeString("LoadAttributeModels", this.LoadAttributeModels.ToString());
            xmlWriter.WriteAttributeString("LoadWorkflowInformation", this.LoadWorkflowInformation.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("WorkflowName", this.WorkflowName);
            xmlWriter.WriteAttributeString("LoadLookupDisplayValues", this.LoadLookupDisplayValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("RequestedRelationshipsList", requestedRelationshipsList);
            xmlWriter.WriteAttributeString("LoadDependentAttributes", this.LoadDependentAttributes.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IgnoreEntityStatus", this.IgnoreEntityStatus.ToString());
            xmlWriter.WriteAttributeString("LoadBlankAttributes", this.LoadBlankAttributes.ToString());
            xmlWriter.WriteAttributeString("LoadComplexChildAttributes", this.LoadComplexChildAttributes.ToString());
            xmlWriter.WriteAttributeString("LoadLookupRowWithValues", this.LoadLookupRowWithValues.ToString());
            xmlWriter.WriteAttributeString("LoadStateValidationAttributes", this.LoadStateValidationAttributes.ToString());
            xmlWriter.WriteAttributeString("LoadBusinessConditions", this.LoadBusinessConditions.ToString());

            // EntityContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            // get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Create a new entity context object.
        /// </summary>
        /// <returns>New entity context instance</returns>
        public IEntityContext Clone()
        {
            var clonedEntityContext = new EntityContext();

            clonedEntityContext._entityModelContext = (EntityModelContext)this._entityModelContext.Clone();
            clonedEntityContext._attributeContext = (AttributeContext)_attributeContext.Clone();

            if (this._entityHierarchyContext != null)
            {
                clonedEntityContext._entityHierarchyContext = (EntityHierarchyContext)this._entityHierarchyContext.Clone();
            }

            if (this._entityExtensionContext != null)
            {
                clonedEntityContext._entityExtensionContext = (EntityExtensionContext)this._entityExtensionContext.Clone();
            }
            
            clonedEntityContext._relationshipContext = this._relationshipContext != null ? this._relationshipContext.Clone() : null;

            clonedEntityContext._locale = this._locale;
            clonedEntityContext._dataLocales = ValueTypeHelper.CloneCollection(this._dataLocales);

            clonedEntityContext._loadEntityProperties = this._loadEntityProperties;
            clonedEntityContext._loadAttributes = this._loadAttributes;
            clonedEntityContext._loadHierarchyRelationships = this._loadHierarchyRelationships;
            clonedEntityContext._loadExtensionRelationships = this._loadExtensionRelationships;

            clonedEntityContext._loadRelationships = this._loadRelationships;
            clonedEntityContext._loadSources = this._loadSources;
            clonedEntityContext._loadWorkflowInformation = this._loadWorkflowInformation;
            clonedEntityContext._workflowName = this._workflowName;
            clonedEntityContext._ignoreEntityStatus = this._ignoreEntityStatus;
            clonedEntityContext._resolveIdsByNames = this._resolveIdsByNames;
            clonedEntityContext._extendedProperties = this._extendedProperties;
            clonedEntityContext._loadBusinessConditions = this._loadBusinessConditions;

            return clonedEntityContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEntityDataContext IEntityDataContext.Clone()
        {
            return (IEntityDataContext)this.Clone();
        }

        /// <summary>
        /// Update current EntityContext with the information provided in another EntityContext coming as parameters
        /// </summary>
        /// <param name="entityContext">EntityContext from which current context is to be updated</param>
        public void UpdateContext(EntityContext entityContext)
        {
            // Do not update ContainerId, CategoryId, EntityTypeId and locale.

            if (entityContext.StateViewId > 0 && !String.IsNullOrWhiteSpace(entityContext.StateViewName))
            {
                this.StateViewId = entityContext.StateViewId;
                this.StateViewName = entityContext.StateViewName;
            }

            if (entityContext.CustomViewId > 0 && !String.IsNullOrWhiteSpace(entityContext.CustomViewName))
            {
                this.CustomViewId = entityContext.CustomViewId;
                this.CustomViewName = entityContext.CustomViewName;
            }

            if (entityContext.LoadAttributes)
            {
                this.LoadAttributes = true;
            }

            if (entityContext.LoadAttributeModels)
            {
                this.LoadAttributeModels = true;
            }

            if (entityContext.LoadHierarchyRelationships)
            {
                this.LoadHierarchyRelationships = true;
            }

            if (entityContext.LoadExtensionRelationships)
            {
                this.LoadExtensionRelationships = true;
            }

            if (entityContext.LoadCreationAttributes)
            {
                this.LoadCreationAttributes = true;
            }

            if (entityContext.LoadEntityProperties)
            {
                this.LoadEntityProperties = true;
            }

            if (entityContext.LoadRelationships)
            {
                this.LoadRelationships = true;
            }

            if (entityContext.LoadSources)
            {
                this.LoadSources = true;
            }

            var sourceRelationshipContext = entityContext._relationshipContext;

            if (sourceRelationshipContext != null)
            {
                if (_relationshipContext == null)
                {
                    _relationshipContext = new RelationshipContext();
                }

                _relationshipContext.LoadRelationshipAttributes = _relationshipContext.LoadRelationshipAttributes || sourceRelationshipContext.LoadRelationshipAttributes;
                _relationshipContext.Level = _relationshipContext.Level > sourceRelationshipContext.Level ? _relationshipContext.Level : sourceRelationshipContext.Level;

                if (sourceRelationshipContext.RelationshipTypeIdList != null && sourceRelationshipContext.RelationshipTypeIdList.Count > 0)
                {
                    if (_relationshipContext.RelationshipTypeIdList == null)
                    {
                        _relationshipContext.RelationshipTypeIdList = new Collection<Int32>();
                    }

                    _relationshipContext.RelationshipTypeIdList = ValueTypeHelper.MergeCollections(_relationshipContext.RelationshipTypeIdList, sourceRelationshipContext.RelationshipTypeIdList);
                }

                if (sourceRelationshipContext.RelationshipTypeNames != null && sourceRelationshipContext.RelationshipTypeNames.Count > 0)
                {
                    if (_relationshipContext.RelationshipTypeNames == null)
                    {
                        _relationshipContext.RelationshipTypeNames = new Collection<String>();
                    }

                    _relationshipContext.RelationshipTypeNames = ValueTypeHelper.MergeCollections(_relationshipContext.RelationshipTypeNames, sourceRelationshipContext.RelationshipTypeNames);
                }
            }

            // If current context's AttributeModelType is different then AttributeModelType in context, then make current AttributeModelype as All
            if (this.AttributeModelType != entityContext.AttributeModelType)
            {
                this.AttributeModelType = Core.AttributeModelType.All;
            }

            if (entityContext.AttributeIdList != null && entityContext.AttributeIdList.Count > 0)
            {
                if (this.AttributeIdList == null)
                    this.AttributeIdList = new Collection<Int32>();

                this.AttributeIdList = ValueTypeHelper.MergeCollections(this.AttributeIdList, entityContext.AttributeIdList);
            }

            if (entityContext.DataLocales != null && entityContext.DataLocales.Count > 0)
            {
                entityContext.DataLocales = ValueTypeHelper.MergeCollections(this.DataLocales, entityContext.DataLocales);
            }

            if (entityContext.AttributeNames != null && entityContext.AttributeNames.Count > 0)
            {
                if (this.AttributeNames == null)
                    this.AttributeNames = new Collection<String>();

                foreach (var attributeName in entityContext.AttributeNames)
                {
                    if (!this.AttributeNames.Contains(attributeName))
                    {
                        this.AttributeNames.Add(attributeName);
                    }
                }
            }

            if (entityContext.AttributeGroupIdList != null && entityContext.AttributeGroupIdList.Count > 0)
            {
                if (this.AttributeGroupIdList == null)
                    this.AttributeGroupIdList = new Collection<Int32>();

                this.AttributeGroupIdList = ValueTypeHelper.MergeCollections(this.AttributeGroupIdList, entityContext.AttributeGroupIdList);
            }

            if (entityContext.LoadWorkflowInformation)
            {
                this.LoadWorkflowInformation = true;
            }

            if (!String.IsNullOrWhiteSpace(entityContext.WorkflowName))
            {
                this.WorkflowName = entityContext.WorkflowName;
            }

            if (entityContext.LoadLookupDisplayValues)
            {
                this.LoadLookupDisplayValues = true;
            }

            if (entityContext.LoadLookupRowWithValues)
            {
                this.LoadLookupRowWithValues = true;
            }

            if (entityContext.LoadDependentAttributes)
            {
                this.LoadDependentAttributes = true;
            }

            if (entityContext.IgnoreEntityStatus)
            {
                this.IgnoreEntityStatus = true;
            }

            this.LoadBlankAttributes = entityContext.LoadBlankAttributes;

            this.LoadComplexChildAttributes = entityContext.LoadComplexChildAttributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntityModelContext GetEntityModelContext()
        {
            return _entityModelContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityModelContext"></param>
        public void SetEntityModelContext(IEntityModelContext entityModelContext)
        {
            _entityModelContext = (EntityModelContext)entityModelContext;
        }

        /// <summary>
        /// Sets the relationship context.
        /// </summary>
        /// <param name="iRelationshipContext">Relationship Context interface.</param>
        public void SetRelationshipContext(IRelationshipContext iRelationshipContext)
        {
            if (iRelationshipContext != null)
            {
                this._relationshipContext = (RelationshipContext)iRelationshipContext;
            }
        }

        /// <summary>
        /// Adds attribute unique identifier in AttributeNameGroupNameList
        /// </summary>
        /// <param name="attributeName">Represents Attribute short name</param>
        public void AddAttribute(String attributeName)
        {
            if (!String.IsNullOrWhiteSpace(attributeName))
            {
                this.AttributeNames.Add(attributeName);
            }
        }

        /// <summary>
        /// Adds attribute unique identifier in AttributeGroupNameList
        /// </summary>
        /// <param name="attributeGroupName">Represents Attribute Group short name</param>
        public void AddAttributeGroupName(String attributeGroupName)
        {
            if (!String.IsNullOrWhiteSpace(attributeGroupName))
            {
                this.AttributeGroupNames.Add(attributeGroupName);
            }
        }

        /// <summary>
        /// Gets the relationship context interface
        /// </summary>
        /// <returns>Relationship Context interface.</returns>
        public IRelationshipContext GetRelationshipContext()
        {
            return (IRelationshipContext)this.RelationshipContext;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">EntityContext Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(EntityContext objectToBeCompared, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }

                if (this.CategoryId != objectToBeCompared.CategoryId)
                {
                    return false;
                }

                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                {
                    return false;
                }

                if (this.AttributeGroupIdList != objectToBeCompared.AttributeGroupIdList)
                {
                    return false;
                }

                if (this.AttributeIdList != objectToBeCompared.AttributeIdList)
                {
                    return false;
                }
            }

            if (this.Locale != objectToBeCompared.Locale)
            {
                return false;
            }

            if (this.LoadEntityProperties != objectToBeCompared.LoadEntityProperties)
            {
                return false;
            }

            if (this.LoadAttributes != objectToBeCompared.LoadAttributes)
            {
                return false;
            }

            if (this.LoadOnlyCurrentValues != objectToBeCompared.LoadOnlyCurrentValues)
            {
                return false;
            }

            if(this.LoadInheritableOnlyAttributes != objectToBeCompared.LoadInheritableOnlyAttributes)
            {
                return false;
            }

            if (this.LoadHierarchyRelationships != objectToBeCompared.LoadHierarchyRelationships)
            {
                return false;
            }

            if (this.LoadExtensionRelationships != objectToBeCompared.LoadExtensionRelationships)
            {
                return false;
            }

            if (this.LoadRelationships != objectToBeCompared.LoadRelationships)
            {
                return false;
            }

            if (this.LoadCreationAttributes != objectToBeCompared.LoadCreationAttributes)
            {
                return false;
            }

            if (this.LoadRequiredAttributes != objectToBeCompared.LoadRequiredAttributes)
            {
                return false;
            }

            if (this.AttributeModelType != objectToBeCompared.AttributeModelType)
            {
                return false;
            }

            if (this.LoadLookupDisplayValues != objectToBeCompared.LoadLookupDisplayValues)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CallDataContext GetNewCallDataContext()
        {
            var callDataContext = new CallDataContext();
            var entityContext = this;

            if (entityContext.ContainerId > 0)
                callDataContext.ContainerIdList.Add(entityContext.ContainerId);

            if (entityContext.EntityTypeId > 0)
                callDataContext.EntityTypeIdList.Add(entityContext.EntityTypeId);

            if (entityContext._relationshipContext != null)
                callDataContext.RelationshipTypeIdList = entityContext._relationshipContext.RelationshipTypeIdList;

            if (entityContext.CategoryId > 0)
                callDataContext.CategoryIdList.Add(entityContext.CategoryId);

            if (entityContext.DataLocales.Count > 0)
                callDataContext.LocaleList = entityContext.DataLocales;

            return callDataContext;
        }

        /// <summary>
        /// Sets the attribute context.
        /// </summary>
        /// <param name="attributeContext">Entity hierarchy Context interface.</param>
        public void SetAttributeContext(IAttributeContext attributeContext)
        {
            if (attributeContext != null)
            {
                this._attributeContext = (AttributeContext)attributeContext;
            }
        }

        /// <summary>
        /// Gets the attribute context interface
        /// </summary>
        /// <returns>Entity attribute context interface.</returns>
        public IAttributeContext GetAttributeContext()
        {
            return this._attributeContext;
        }

        /// <summary>
        /// Sets the entity hierarchy context.
        /// </summary>
        /// <param name="iEntityHierarchyContext">Entity hierarchy Context interface.</param>
        public void SetEntityHierarchyContext(IEntityHierarchyContext iEntityHierarchyContext)
        {
            if (iEntityHierarchyContext != null)
            {
                this._entityHierarchyContext = (EntityHierarchyContext)iEntityHierarchyContext;
            }
        }

        /// <summary>
        /// Gets the entity hierarchy context interface
        /// </summary>
        /// <returns>Entity hierarchy Context interface.</returns>
        public IEntityHierarchyContext GetEntityHierarchyContext()
        {
            return (IEntityHierarchyContext)this._entityHierarchyContext;
        }

        /// <summary>
        /// Sets the entity extension context.
        /// </summary>
        /// <param name="iEntityExtensionContext">Entity extension Context interface.</param>
        public void SetEntityExtensionContext(IEntityExtensionContext iEntityExtensionContext)
        {
            if (iEntityExtensionContext != null)
            {
                this._entityExtensionContext = (EntityExtensionContext)iEntityExtensionContext;
            }
        }

        /// <summary>
        /// Gets the entity extension context interface
        /// </summary>
        /// <returns>Entity extension Context interface.</returns>
        public IEntityExtensionContext GetEntityExtensionContext()
        {
            return (IEntityExtensionContext)this._entityExtensionContext;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="categoryId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="locale"></param>
        /// <param name="datalocales"></param>
        /// <param name="loadEntityProperties"></param>
        /// <param name="loadAttributes"></param>
        /// <param name="loadHierarchyRelationships"></param>
        /// <param name="loadExtensionRelationships"></param>
        /// <param name="attributeGroupIdList"></param>
        /// <param name="attributeIdList"></param>
        /// <param name="loadRelationships"></param>
        /// <param name="loadRelationshipAttributes"></param>
        /// <param name="relationshipTypeIdList"></param>
        /// <param name="loadCreationAttributes"></param>
        /// <param name="loadRequiredAttributes"></param>
        /// <param name="attributeModelType"></param>
        private void InitialiseFields(Int32 containerId, Int64 categoryId, Int32 entityTypeId, LocaleEnum locale, Collection<LocaleEnum> datalocales,
            Boolean loadEntityProperties, Boolean loadAttributes, Boolean loadHierarchyRelationships, Boolean loadExtensionRelationships, Collection<Int32> attributeGroupIdList, Collection<Int32> attributeIdList,
            Boolean loadRelationships, Boolean loadRelationshipAttributes, Collection<Int32> relationshipTypeIdList,
            Boolean loadCreationAttributes, Boolean loadRequiredAttributes, AttributeModelType attributeModelType)
        {
            this._entityModelContext.ContainerId = containerId;
            this._entityModelContext.CategoryId = categoryId;
            this._entityModelContext.EntityTypeId = entityTypeId;
            this._dataLocales = datalocales;
            this._locale = locale;
            this._loadEntityProperties = loadEntityProperties;
            this._loadAttributes = loadAttributes;
            this._loadHierarchyRelationships = loadHierarchyRelationships;
            this._loadExtensionRelationships = loadExtensionRelationships;
            this._attributeContext.AttributeGroupIdList = attributeGroupIdList;
            this._attributeContext.AttributeIdList = attributeIdList;
            this._loadRelationships = loadRelationships;

            if (loadRelationshipAttributes || (relationshipTypeIdList != null && relationshipTypeIdList.Count > 0))
            {
                this.RelationshipContext.LoadRelationshipAttributes = loadRelationshipAttributes;
                this.RelationshipContext.RelationshipTypeIdList = relationshipTypeIdList;
            }

            this._attributeContext.LoadCreationAttributes = loadCreationAttributes;
            this._attributeContext.LoadRequiredAttributes = loadRequiredAttributes;
            this._attributeContext.AttributeModelType = attributeModelType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityContext")
                    {
                        #region Read EntityContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("ContainerQualifierId"))
                            {
                                this.ContainerQualifierId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("ContainerQualifierName"))
                            {
                                this.ContainerQualifierName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ContainerName"))
                            {
                                this.ContainerName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("EntityTypeId"))
                            {
                                this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("EntityTypeName"))
                            {
                                this.EntityTypeName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CategoryId"))
                            {
                                this.CategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("CategoryPath"))
                            {
                                this.CategoryPath = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Locale"))
                            {
                                String strLocale = reader.GetAttribute("Locale");
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
                                this.Locale = locale;
                            }

                            if (reader.MoveToAttribute("DataLocales"))
                            {
                                String strLocales = reader.GetAttribute("DataLocales");
                                this.DataLocales = ValueTypeHelper.SplitStringToLocaleEnumCollection(strLocales, ',');
                            }

                            if (reader.MoveToAttribute("StateViewId"))
                            {
                                this.StateViewId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("StateViewName"))
                            {
                                this.StateViewName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CustomViewId"))
                            {
                                this.CustomViewId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("CustomViewName"))
                            {
                                this.CustomViewName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LoadEntityProperties"))
                            {
                                this.LoadEntityProperties = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadAttributes"))
                            {
                                this.LoadAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadOnlyCurrentValues"))
                            {
                                this.LoadOnlyCurrentValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadHierarchyRelationships"))
                            {
                                this.LoadHierarchyRelationships = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadExtensionRelationships"))
                            {
                                this.LoadExtensionRelationships = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRelationships"))
                            {
                                this.LoadRelationships = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadSources"))
                            {
                                this.LoadSources = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRelationshipAttributes"))
                            {
                                Boolean loadRelationshipAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());

                                if (loadRelationshipAttributes)
                                {
                                    this.RelationshipContext.LoadRelationshipAttributes = true;
                                }
                            }

                            if (reader.MoveToAttribute("LoadCreationAttributes"))
                            {
                                this.LoadCreationAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRequiredAttributes"))
                            {
                                this.LoadRequiredAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("AttributeModelType"))
                            {
                                AttributeModelType attributeModeType = AttributeModelType.All;
                                Enum.TryParse(reader.ReadContentAsString(), true, out attributeModeType);
                                this.AttributeModelType = attributeModeType;
                            }

                            if (reader.MoveToAttribute("AttributeGroupIdList"))
                            {
                                this.AttributeGroupIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("AttributeIdList"))
                            {
                                this.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("RelationshipTypeIdList"))
                            {
                                Collection<Int32> relationshipTypeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');

                                if (relationshipTypeIdList != null && relationshipTypeIdList.Count > 0)
                                {
                                    this.RelationshipContext.RelationshipTypeIdList = relationshipTypeIdList;
                                }
                            }

                            if (reader.MoveToAttribute("RelationshipLevelsToBeConsidered"))
                            {
                                Int16 relationshipLevels = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), 0);

                                if (relationshipLevels > 0)
                                {
                                    this.RelationshipContext.Level = relationshipLevels;
                                }
                            }

                            if (reader.MoveToAttribute("LoadAttributeModels"))
                            {
                                this.LoadAttributeModels = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadWorkflowInformation"))
                            {
                                this.LoadWorkflowInformation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("WorkflowName"))
                            {
                                this.WorkflowName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LoadLookupDisplayValues"))
                            {
                                this.LoadLookupDisplayValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadDependentAttributes"))
                            {
                                this.LoadDependentAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("IgnoreEntityStatus"))
                            {
                                this.IgnoreEntityStatus = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadBlankAttributes"))
                            {
                                this.LoadBlankAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadComplexChildAttributes"))
                            {
                                this.LoadComplexChildAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadLookupRowWithValues"))
                            {
                                this.LoadLookupRowWithValues = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadBusinessConditions"))
                            {
                                this.LoadBusinessConditions = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeContext")
                    {
                        #region Read AttributeContext properties

                        String attributeContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(attributeContextXml))
                        {
                            this._attributeContext = new AttributeContext(attributeContextXml);
                        }

                        #endregion Read AttributeContext properties
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipContext")
                    {
                        #region Read RelationshipContext properties

                        String relationshipContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(relationshipContextXml))
                        {
                            this.RelationshipContext = new RelationshipContext(relationshipContextXml);
                        }

                        #endregion Read RelationshipContext properties
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHierarchyContext")
                    {
                        #region Read EntityHierarchyContext Properties

                        String entityHierarchyContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(entityHierarchyContextXml))
                        {
                            this.EntityHierarchyContext = new EntityHierarchyContext(entityHierarchyContextXml);
                        }

                        #endregion Read EntityHierarchyContext Properties
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityExtensionContext")
                    {
                        #region Read EntityExtensionContext Properties

                        String entityExtensionContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(entityExtensionContextXml))
                        {
                            this.EntityExtensionContext = new EntityExtensionContext(entityExtensionContextXml);
                        }

                        #endregion Read EntityHierarchyContext Properties
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationships"></param>
        /// <param name="relationshipContext"></param>
        private void PopulateRelationshipTypeIdListAndLocales(RelationshipCollection relationships, RelationshipContext relationshipContext)
        {
            if (relationships != null && relationships.Count > 0)
            {
                foreach (Relationship relationship in relationships)
                {
                    AttributeCollection relationshipAttributes = relationship.RelationshipAttributes;
                    Int32 relationshipTypeId = relationship.RelationshipTypeId;

                    if (!relationshipContext.RelationshipTypeIdList.Contains(relationshipTypeId))
                    {
                        relationshipContext.RelationshipTypeIdList.Add(relationshipTypeId);
                    }

                    if (relationshipAttributes != null && relationshipAttributes.Count > 0)
                    {
                        foreach (Attribute relationshipAttribute in relationshipAttributes)
                        {
                            LocaleEnum relationshipLocale = relationshipAttribute.Locale;

                            if (!relationshipContext.DataLocales.Contains(relationshipLocale))
                            {
                                relationshipContext.DataLocales.Add(relationshipLocale);
                            }
                        }
                    }

                    PopulateRelationshipTypeIdListAndLocales(relationship.RelationshipCollection, relationshipContext);
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}