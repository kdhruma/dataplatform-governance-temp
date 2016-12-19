using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the cache status of an entity.
    /// </summary>
    public class EntityCacheStatus : MDMObject, IDataProcessorEntity
    {
        #region Fields

        /// <summary>
        /// Unique Id representing table
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field for the id of Entity cache status
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Indicates the catalog Id of the Entity cache status
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Holds the cache status as type 'EntityCacheComponentEnum'.
        /// </summary>
        private EntityCacheComponentEnum _entityCacheComponentEnum = EntityCacheComponentEnum.None;

        /// <summary>
        /// Indicates the weightage of the entity from the processing point of view.
        /// </summary>
        private Int32 _weightage = 10000;//TODO: Temporarily hard coded.

        /// <summary>
        /// Indicates if cache loading is required for an entity.
        /// </summary>
        private Boolean _isCacheLoadingRequired = false;

        /// <summary>
        /// Indicates if cache status is modified for an entity.
        /// </summary>
        private Boolean _isCacheStatusUpdated = false;

        /// <summary>
        /// Indicates the PK of activity Log which is referred to Entity cache status
        /// </summary>
        private Int64 _entityActivityLogId = 0;

        /// <summary>
        /// Indicates if the entity is a category.
        /// </summary>
        private Boolean _isCategory = false;

        /// <summary>
        /// Indicates if the entity is a category.
        /// </summary>
        private Boolean _overWriteCacheStatus = false;

        /// <summary>
        /// Action performed like attribute update,reclassify etc.
        /// Note: This is used only for the logging the exception in the Processor
        /// </summary>
        private EntityActivityList _performedAction = EntityActivityList.EntityCacheLoad;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityCacheStatus() : base()
        {            
        }

        /// <summary>
        /// Constructor which takes xml as input parameters
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityCacheStatus(String valuesAsXml)
        {
            LoadEntityCacheStatus(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property Denoting Weightage
        /// </summary>
        public Int32 Weightage 
        {
            get
            {
                return this._weightage;
            }
            set
            {
                this._weightage = value;
            }
        }
        
        /// <summary>
        /// Action performed like attribute update,reclassify etc.
        /// Note: This is used only for the logging the exception in the Processor
        /// </summary>
        [DataMember]
        public EntityActivityList PerformedAction
        {
            get
            {
                return _performedAction;
            }
            set
            {
                _performedAction = value;
            }
        }

        /// <summary>
        /// Property for the id of an Entity cache status
        /// </summary>
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Field for the id of Entity.
        /// </summary>
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Indicates the container Id of the Entity cache status
        /// </summary>
        public Int32 ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Indicates if the cache loading is required for an entity.
        /// </summary>
        public Boolean IsCacheLoadingRequired
        {
            get { return _isCacheLoadingRequired; }
            set { _isCacheLoadingRequired = value; }
        }

        /// <summary>
        /// Specifies the cache status.
        /// </summary>
        public Int32 CacheStatus
        {
            get
            {
                return (Int32)_entityCacheComponentEnum;
            }
            set
            {
                _entityCacheComponentEnum = (EntityCacheComponentEnum)value;
            }
        }

        /// <summary>
        /// Indicates if the entity is a category.
        /// </summary>
        public Boolean IsCategory
        {
            get { return _isCategory; }
            set { _isCategory = value; }
        }

        /// <summary>
        /// Indicates if the entity cache status can be overwritten in to the database.
        /// The process operation after entity process will do a merge update with database, but when invoked from entity cache processor, the value should be overwritten.
        /// </summary>
        public Boolean OverWriteCacheStatus
        {
            get { return _overWriteCacheStatus; }
            set { _overWriteCacheStatus = value; }
        }

        /// <summary>
        /// Indicates the PK of activity Log which is referred to Entity cache status
        /// </summary>
        public Int64 EntityActivityLogId
        {
            get
            {
                return this._entityActivityLogId;
            }
            set
            {
                this._entityActivityLogId = value;
            }
        }

        #region Individual Cache Status Properties

        /// <summary>
        /// Specifies if the Base Entity cache is dirty.
        /// </summary>
        public Boolean IsBaseEntityCacheDirty
        {
            get
            {
                return IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum.BaseEntity);
            }
            set
            {
                UpdateCacheComponentStatus(EntityCacheComponentEnum.BaseEntity, value);
            }
        }

        /// <summary>
        /// Specifies if the Inherited Attribute cache is dirty.
        /// </summary>
        public Boolean IsInheritedAttributesCacheDirty
        {
            get
            {
                return IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum.InheritedAttributes);
            }
            set
            {
                UpdateCacheComponentStatus(EntityCacheComponentEnum.InheritedAttributes, value);
            }
        }

        /// <summary>
        /// Specifies if the Overridden Attribute cache is dirty.
        /// </summary>
        public Boolean IsOverriddenAttributesCacheDirty
        {
            get
            {
                return IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum.OverriddenAttributes);
            }
            set
            {
                UpdateCacheComponentStatus(EntityCacheComponentEnum.OverriddenAttributes, value);
            }
        }

        /// <summary>
        /// Specifies if the Relationships cache is dirty.
        /// </summary>
        public Boolean IsRelationshipCacheDirty
        {
            get
            {
                return IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum.Relationships);
            }
            set
            {
                UpdateCacheComponentStatus(EntityCacheComponentEnum.Relationships, value);
            }
        }

        /// <summary>
        /// Specifies if the Extensions cache is dirty.
        /// </summary>
        public Boolean IsExtensionsCacheDirty
        {
            get
            {
                return IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum.Extensions);
            }
            set
            {
                UpdateCacheComponentStatus(EntityCacheComponentEnum.Extensions, value);
            }
        }

        /// <summary>
        /// Specifies if the Hierarchies cache is dirty.
        /// </summary>
        public Boolean IsHierarchyCacheDirty
        {
            get
            {
                return IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum.Hierarchies);
            }
            set
            {
                UpdateCacheComponentStatus(EntityCacheComponentEnum.Hierarchies, value);
            }
        }

        /// <summary>
        /// Specifies if the Work flow cache is dirty.
        /// </summary>
        public Boolean IsWorkflowCacheDirty
        {
            get
            {
                return IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum.Workflow);
            }
            set
            {
                UpdateCacheComponentStatus(EntityCacheComponentEnum.Workflow, value);
            }
        }

        /// <summary>
        /// Indicates if the cache status is updated for an entity.
        /// </summary>
        public Boolean IsCacheStatusUpdated
        {
            get { return _isCacheStatusUpdated; }
            set { _isCacheStatusUpdated = value; }
        }        

        #endregion

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Builds the Entity cache status from the xml string.
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadEntityCacheStatus(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityCacheStatus")
                        {
                            #region Read ImpactedEntity Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("CacheStatus"))
                                {
                                    this.CacheStatus = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("IsCacheLoadingRequired"))
                                {
                                    this.IsCacheLoadingRequired = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("IsCategory"))
                                {
                                    this.IsCategory = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("Priority"))
                                {
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.Action = action;
                                }
                                if (reader.MoveToAttribute("EntityActivityLogId"))
                                {
                                    this.EntityActivityLogId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                            }

                            #endregion
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
        }

        /// <summary>
        /// Get XML representation of Entity cache status object
        /// </summary>
        /// <returns>XML representation of Entity cache status object</returns>
        public override String ToXml()
        {
            String entityCacheStatusXml = String.Empty;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("EntityCacheStatus");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("CacheStatus", this.CacheStatus.ToString());
                    xmlWriter.WriteAttributeString("IsCacheLoadingRequired", this.IsCacheLoadingRequired.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("IsCategory", this.IsCategory.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("Priority", this.Weightage.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("EntityActivityLogId", this.EntityActivityLogId.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());

                    //EntityCacheStatus node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    entityCacheStatusXml = stringWriter.ToString();
                }
            }

            return entityCacheStatusXml;
        }

        /// <summary>
        /// Get XML representation of Entity Cache Status object
        /// </summary>
        /// <param name="serializationType">Indicates type of serialization</param>
        /// <returns>XML representation of Entity Cache Status object</returns>
        public override String ToXml(ObjectSerialization serializationType)
        {
            String entityCacheStatusXml = this.ToXml();
            return entityCacheStatusXml;
        }

        /// <summary>
        /// Resets the Cache status of an entity and turns off the cache loading required flag.
        /// </summary>
        public void ResetCacheStatus()
        {
            _entityCacheComponentEnum = EntityCacheComponentEnum.None;
            _isCacheStatusUpdated = true;
            _overWriteCacheStatus = true;
            _isCacheLoadingRequired = false;
        }

        /// <summary>
        /// Marks the cache dirty flag for all components of an entity.
        /// </summary>
        public void MarkAllCacheDirty()
        {
            _entityCacheComponentEnum = (EntityCacheComponentEnum.BaseEntity | EntityCacheComponentEnum.InheritedAttributes | EntityCacheComponentEnum.OverriddenAttributes
                | EntityCacheComponentEnum.Extensions | EntityCacheComponentEnum.Hierarchies | EntityCacheComponentEnum.Relationships);

            _isCacheLoadingRequired = true;
            _isCacheStatusUpdated = true;
        }

        #endregion

        #region Private Methods

        private Boolean IsEntityCacheComponentStatusDirty(EntityCacheComponentEnum entityCacheComponent)
        {
            return ((_entityCacheComponentEnum & entityCacheComponent) == entityCacheComponent);
        }

        private void UpdateCacheComponentStatus(EntityCacheComponentEnum entityCacheComponent, Boolean isValueSet)
        {
            // Cache loading needs to be set only if the EntityCacheComponent changes.
            if (isValueSet && !IsEntityCacheComponentStatusDirty(entityCacheComponent))
            {
                _entityCacheComponentEnum |= entityCacheComponent;                
                _isCacheStatusUpdated = true;
                _isCacheLoadingRequired = true;
            }
        }

        #endregion

        #endregion Methods
    }
}
