using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ImpactedEntity : MDMObject, IImpactedEntity, IDataProcessorEntity
    {
        #region Fields

        /// <summary>
        /// Unique Id representing table
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field for the id of impacted Entity
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Indicates the catalog Id of the Impacted entity
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field indicating entity short name
        /// </summary>
        private String _entityName = String.Empty;

        /// <summary>
        /// Field indicating entity Long name
        /// </summary>
        private String _entityLongName = String.Empty;

        /// <summary>
        /// Indicates the priority of the entity from the processing point of view.
        /// </summary>
        private Int32 _priority = 0;

        /// <summary>
        /// Indicates if a given entity needs to be updated in the cache or not.
        /// </summary>
        private Boolean _isCacheDirty = false;

        /// <summary>
        /// Indicates context for entity denorm 
        /// </summary>
        private String _context = String.Empty;

        /// <summary>
        /// Indicates if the entity denorm has to be processed for the given entity.
        /// </summary>
        private Boolean _isEntityDenormRequired = false;

        /// <summary>
        /// 
        /// </summary>
        private Collection<EntityActivityList> _performedActionList = new Collection<EntityActivityList>();

        /// <summary>
        /// Contains the list of a attribute ids which got impacted.
        /// </summary>
        private Collection<Int32> _impactedAttributes = new Collection<Int32>();

        /// <summary>
        /// Contains the list of a locales for impacted attributes.
        /// </summary>
        private Collection<LocaleEnum> _impactedAttributeLocales = new Collection<LocaleEnum>();

        /// <summary>
        /// Indicates if the entity denorm is currently in progress.
        /// </summary>
        private Boolean _isEntityDenormInProcess = false;

        /// <summary>
        /// 
        /// </summary>
        private Collection<EntityActivityList> _shelvedPerformedActionList = new Collection<EntityActivityList>();

        /// <summary>
        /// When the attribute denorm is in progress, this attribute list is updated in this field.
        /// </summary>
        private Collection<Int32> _shelvedAttributes = new Collection<Int32>();

        /// <summary>
        /// When the attribute denorm is currently in progress, the locales are updated in this field
        /// </summary>
        private Collection<LocaleEnum> _shelvedAttributeLocales = new Collection<LocaleEnum>();

        /// <summary>
        /// Contains the list of impacted relationship ids 
        /// </summary>
        private Collection<Int64> _impactedRelationships = new Collection<Int64>();

        /// <summary>
        /// Contains the list of locales for impacted relationships
        /// </summary>
        private Collection<LocaleEnum> _impactedRelationshipLocales = new Collection<LocaleEnum>();

        /// <summary>
        /// When the Relationship denorm is in progress, this relationship list is updated in this field
        /// </summary>
        private Collection<Int64> _shelvedRelationships = new Collection<Int64>();

        /// <summary>
        /// When the Relationship denorm is currently in progress, the locales are updated in this field.
        /// </summary>
        private Collection<LocaleEnum> _shelvedRelationshipLocales = new Collection<LocaleEnum>();

        /// <summary>
        /// indicates the PK of Impacted Entity Log which is referred to impacted entity
        /// </summary>
        private Int64 _entityActivityLogId = 0;

        /// <summary>
        /// Action performed like attribute update,reclassify etc.
        /// Note: This is used only for the logging the exception in the Processor
        /// </summary>
        private EntityActivityList _performedAction = EntityActivityList.AttributeUpdate;

        #endregion Fields

        #region Constructor
        
        /// <summary>
        /// 
        /// </summary>
        public ImpactedEntity()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public ImpactedEntity(String valuesAsXml)
        {
            LoadImpactedEntity(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property for the id of an Entity
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Field for the id of impacted Entity
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Indicates the catalog Id of the Impacted entity
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
        /// Property indicating entity short name
        /// </summary>
        [DataMember]
        public String EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        /// <summary>
        /// Property indicating entity Long name
        /// </summary>
        [DataMember]
        public String EntityLongName
        {
            get { return _entityLongName; }
            set { _entityLongName = value; }
        }

        /// <summary>
        /// Indicates the priority of the entity from the processing point of view.
        /// </summary>
        [DataMember]
        public Int32 Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        /// <summary>
        /// indicates weightage of an impacted entity. This is same as Priority for this object
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        /// <summary>
        /// Indicates if a given entity needs to be updated in the cache or not.
        /// </summary>
        [DataMember]
        public Boolean IsCacheDirty
        {
            get { return _isCacheDirty; }
            set { _isCacheDirty = value; }
        }

        /// <summary>
        /// Indicates context for entity denorm
        /// </summary>
        [DataMember]
        public String Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        /// Indicates if the entity denorm has to be processed for the given entity.
        /// </summary>
        [DataMember]
        public Boolean IsEntityDenormRequired
        {
            get { return _isEntityDenormRequired; }
            set { _isEntityDenormRequired = value; }
        }

        /// <summary>
        /// Contains the list of actions performed
        /// </summary>
        [DataMember]
        public Collection<EntityActivityList> PerformedActionList
        {
            get
            {
                if (_performedActionList != null && _performedActionList.Count > 0)
                {
                    _performedActionList = new Collection<EntityActivityList>(_performedActionList.Where(action => action != EntityActivityList.UnKnown).Distinct().ToList<EntityActivityList>());
                }
                else
                {
                    _performedActionList = new Collection<EntityActivityList>();
                }

                return _performedActionList;
            }
            set { _performedActionList = value; }
        }

        /// <summary>
        /// Contains the list of a attribute ids which got impacted.
        /// </summary>
        [DataMember]
        public Collection<Int32> ImpactedAttributes
        {
            get
            {
                if (_impactedAttributes != null && _impactedAttributes.Count > 0)
                {
                    _impactedAttributes = new Collection<Int32>(_impactedAttributes.Where(attr => attr > 0).Distinct().ToList<Int32>());
                }
                else
                {
                    _impactedAttributes = new Collection<Int32>();
                }

                return _impactedAttributes;
            }
            set { _impactedAttributes = value; }
        }

        /// <summary>
        /// Contains the list of a locales for impacted attributes.
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> ImpactedAttributeLocales
        {
            get
            {
                if (_impactedAttributeLocales != null && _impactedAttributeLocales.Count > 0)
                {
                    _impactedAttributeLocales = new Collection<LocaleEnum>(_impactedAttributeLocales.Where(l => l != LocaleEnum.UnKnown).Distinct().ToList<LocaleEnum>());
                }
                else
                {
                    _impactedAttributeLocales = new Collection<LocaleEnum>();
                }

                return _impactedAttributeLocales;
            }
            set { _impactedAttributeLocales = value; }
        }

        /// <summary>
        /// Indicates if the entity denorm is currently in progress.
        /// </summary>
        [DataMember]
        public Boolean IsEntityDenormInProcess
        {
            get { return _isEntityDenormInProcess; }
            set { _isEntityDenormInProcess = value; }
        }

        /// <summary>
        /// When the attribute denorm is in progress, action list is updated in this field.
        /// </summary>
        [DataMember]
        public Collection<EntityActivityList> ShelvedPerformedActionList
        {
            get
            {
                if (_shelvedPerformedActionList != null && _shelvedPerformedActionList.Count > 0)
                {
                    _shelvedPerformedActionList = new Collection<EntityActivityList>(_shelvedPerformedActionList.Where(action => action != EntityActivityList.UnKnown).Distinct().ToList<EntityActivityList>());
                }
                else
                {
                    _shelvedPerformedActionList = new Collection<EntityActivityList>();
                }

                return _shelvedPerformedActionList;
            }
            set { _shelvedPerformedActionList = value; }
        }

        /// <summary>
        /// When the attribute denorm is in progress, attribute list is updated in this field.
        /// </summary>
        [DataMember]
        public Collection<Int32> ShelvedAttributes
        {
            get
            {
                if (this._shelvedAttributes != null && this._shelvedAttributes.Count > 0)
                {
                    this._shelvedAttributes = new Collection<Int32>(this._shelvedAttributes.Where(attr => attr > 0).Distinct().ToList<Int32>());
                }
                else
                {
                    this._shelvedAttributes = new Collection<Int32>();
                }

                return this._shelvedAttributes;
            }
            set { _shelvedAttributes = value; }
        }

        /// <summary>
        /// When the attribute denorm is currently in progress, the locales are updated in this field
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> ShelvedAttributeLocales
        {
            get
            {
                if (this._shelvedAttributeLocales != null && this._shelvedAttributeLocales.Count > 0)
                {
                    this._shelvedAttributeLocales = new Collection<LocaleEnum>(this._shelvedAttributeLocales.Where(l => l != LocaleEnum.UnKnown).Distinct().ToList<LocaleEnum>());
                }
                else
                {
                    this._shelvedAttributeLocales = new Collection<LocaleEnum>();
                }

                return this._shelvedAttributeLocales;
            }
            set { _shelvedAttributeLocales = value; }
        }

        /// <summary>
        /// Contains the list of impacted relationship ids 
        /// </summary>
        [DataMember]
        public Collection<Int64> ImpactedRelationships
        {
            get
            {
                if (this._impactedRelationships != null && this._impactedRelationships.Count > 0)
                {
                    this._impactedRelationships = new Collection<Int64>(this._impactedRelationships.Distinct().ToList<Int64>());
                }
                else
                {
                    this._impactedRelationships= new Collection<Int64>();
                }

                return this._impactedRelationships;
            }
            set { _impactedRelationships = value; }
        }

        /// <summary>
        /// Contains the list of locales for impacted relationships
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> ImpactedRelationshipLocales
        {
            get
            {
                if (this._impactedRelationshipLocales != null && this._impactedRelationshipLocales.Count > 0)
                {
                    this._impactedRelationshipLocales = new Collection<LocaleEnum>(this._impactedRelationshipLocales.Where(l => l != LocaleEnum.UnKnown).Distinct().ToList<LocaleEnum>());
                }
                else
                {
                    this._impactedRelationshipLocales = new Collection<LocaleEnum>();
                }

                return this._impactedRelationshipLocales;
            }
            set { _impactedRelationshipLocales = value; }
        }

        /// <summary>
        /// When the Relationship denorm is in progress, this relationship list is updated in this field
        /// </summary>
        [DataMember]
        public Collection<Int64> ShelvedRelationships
        {
            get
            {
                if (this._shelvedRelationships != null && this._shelvedRelationships.Count > 0)
                {
                    this._shelvedRelationships = new Collection<Int64>(this._shelvedRelationships.Distinct().ToList<Int64>());
                }
                else
                {
                    this._shelvedRelationships = new Collection<Int64>();
                }

                return this._shelvedRelationships;
            }
            set { _shelvedRelationships = value; }
        }

        /// <summary>
        /// When the Relationship denorm is currently in progress, the locales are updated in this field.
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> ShelvedRelationshipLocales
        {
            get
            {
                if (this._shelvedRelationshipLocales != null && this._shelvedRelationshipLocales.Count > 0)
                {
                    this._shelvedRelationshipLocales = new Collection<LocaleEnum>(this._shelvedRelationshipLocales.Where(l => l != LocaleEnum.UnKnown).Distinct().ToList<LocaleEnum>());
                }
                else
                {
                    this._shelvedRelationshipLocales = new Collection<LocaleEnum>();
                }

                return this._shelvedRelationshipLocales;
            }
            set { _shelvedRelationshipLocales = value; }
        }

        /// <summary>
        /// indicates the PK of Impacted Entity Log which is referred to impacted entity
        /// </summary>
        [DataMember]
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

        /// <summary>
        /// Action performed like attribute update,reclassify etc.
        /// Note: This is used only for the logging the exception in the Processor
        /// </summary>
        [DataMember]
        public EntityActivityList PerformedAction
        {
            get
            {
                return EntityActivityList.AttributeUpdate;
            }
            set
            {
                _performedAction = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadImpactedEntity(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ImpactedEntity")
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
                                if (reader.MoveToAttribute("EntityName"))
                                {
                                    this.EntityName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("EntityLongName"))
                                {
                                    this.EntityLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("IsCacheDirty"))
                                {
                                    this.IsCacheDirty = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("Context"))
                                {
                                    this.Context = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Priority"))
                                {
                                    this.Priority = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }
                                if (reader.MoveToAttribute("IsAttributeDenormRequired"))
                                {
                                    this.IsEntityDenormRequired = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("ActionList"))
                                {
                                    //TODO:: Read here..
                                }
                                if (reader.MoveToAttribute("ImpactedAttributes"))
                                {
                                    this.ImpactedAttributes = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("ImpactedAttributeLocales"))
                                {
                                    this.ImpactedAttributeLocales = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("IsAttributeDenormInProcess"))
                                {
                                    this.IsEntityDenormInProcess = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("ShelvedActionList"))
                                {
                                    //TODO:: Read here..
                                }
                                if (reader.MoveToAttribute("ShelvedAttributes"))
                                {
                                    this.ShelvedAttributes = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("ShelvedAttributeLocales"))
                                {
                                    this.ShelvedAttributeLocales = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("ImpactedRelationships"))
                                {
                                    this.ImpactedRelationships = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("ImpactedRelationshipLocales"))
                                {
                                    this.ImpactedRelationshipLocales = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("ShelvedRelationships"))
                                {
                                    this.ShelvedRelationships = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("ShelvedRelationshipLocales"))
                                {
                                    this.ImpactedRelationshipLocales = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
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
        /// Get XML representation of Entity object
        /// </summary>
        /// <returns>XML representation of Entity object</returns>
        public override String ToXml()
        {
            String entityXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            String performedActionList = String.Empty;
            String attributeIdList = String.Empty;
            String attributeLocaleList = String.Empty;
            String relationshipIdList = String.Empty;
            String relationshipLocaleList = String.Empty;
            String shelvedPerformedActionList = String.Empty;
            String shelvedAttributeIdList = String.Empty;
            String shelvedRelationshipsIdList = String.Empty;
            String shelvedAttributeLocaleList = String.Empty;
            String shelvedRelationshipsLocaleList = String.Empty;
            
            if (this.PerformedActionList != null && this.PerformedActionList.Count > 0)
            {
                performedActionList = ValueTypeHelper.JoinCollection(this.PerformedActionList, ",");
            }

            if (this.ImpactedAttributes != null && this.ImpactedAttributes.Count > 0)
            {
                attributeIdList = ValueTypeHelper.JoinCollection(this.ImpactedAttributes, ",");
            }

            if (this.ImpactedAttributeLocales != null && this.ImpactedAttributeLocales.Count > 0)
            {
                StringBuilder sbLocaleList = new StringBuilder();

                foreach (LocaleEnum locale in ImpactedAttributeLocales.Where(l => l != LocaleEnum.UnKnown))
                {
                    if (sbLocaleList.Length > 0)
                        sbLocaleList.Append(",");

                    sbLocaleList.Append(locale);
                }

                attributeLocaleList = sbLocaleList.ToString();
            }

            if (this.ShelvedPerformedActionList != null && this.ShelvedPerformedActionList.Count > 0)
            {
                shelvedPerformedActionList = ValueTypeHelper.JoinCollection(this.ShelvedPerformedActionList, ",");
            }

            if (this.ShelvedAttributes != null && this.ShelvedAttributes.Count > 0)
            {
                shelvedAttributeIdList = ValueTypeHelper.JoinCollection(this.ShelvedAttributes, ",");
            }

            if (this.ShelvedAttributeLocales != null && this.ShelvedAttributeLocales.Count > 0)
            {
                StringBuilder sbLocaleList = new StringBuilder();

                foreach (LocaleEnum locale in this.ShelvedAttributeLocales.Where(l => l != LocaleEnum.UnKnown))
                {
                    if (sbLocaleList.Length > 0)
                        sbLocaleList.Append(",");

                    sbLocaleList.Append(locale);
                }

                shelvedAttributeLocaleList = sbLocaleList.ToString();
            }

            if (this.ImpactedRelationships != null && this.ImpactedRelationships.Count > 0)
            {
                relationshipIdList = ValueTypeHelper.JoinCollection(this.ImpactedRelationships, ",");
            }

            if (this.ImpactedRelationshipLocales != null && this.ImpactedRelationshipLocales.Count > 0)
            {
                StringBuilder sbLocaleList = new StringBuilder();

                foreach (LocaleEnum locale in this.ImpactedRelationshipLocales.Where(l => l != LocaleEnum.UnKnown))
                {
                    if (sbLocaleList.Length > 0)
                        sbLocaleList.Append(",");

                    sbLocaleList.Append(locale);
                }

                relationshipLocaleList = sbLocaleList.ToString();
            }

            if (this.ShelvedRelationships != null && this.ShelvedRelationships.Count > 0)
            {
                shelvedRelationshipsIdList = ValueTypeHelper.JoinCollection(this.ShelvedRelationships, ",");
            }

            if (this.ShelvedRelationshipLocales != null && this.ShelvedRelationshipLocales.Count > 0)
            {
                StringBuilder sbLocaleList = new StringBuilder();

                foreach (LocaleEnum locale in ShelvedRelationshipLocales.Where(l => l != LocaleEnum.UnKnown))
                {
                    if (sbLocaleList.Length > 0)
                        sbLocaleList.Append(",");

                    sbLocaleList.Append(locale);
                }

                shelvedAttributeLocaleList = sbLocaleList.ToString();
            }

            xmlWriter.WriteStartElement("ImpactedEntity");

            #region write impacted entity meta data for Full Xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("EntityName", this.EntityName);
            xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName);
            xmlWriter.WriteAttributeString("IsCacheDirty", this.IsCacheDirty.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("Context", this.Context);
            xmlWriter.WriteAttributeString("Priority", this.Priority.ToString());

            xmlWriter.WriteAttributeString("IsAttributeDenormRequired", this.IsEntityDenormRequired.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("PerformedActionList", performedActionList);
            xmlWriter.WriteAttributeString("ImpactedAttributes", attributeIdList);
            xmlWriter.WriteAttributeString("ImpactedAttributeLocales", attributeLocaleList);
            xmlWriter.WriteAttributeString("IsAttributeDenormInProcess", this.IsEntityDenormInProcess.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ShelvedPerformedActionList", shelvedPerformedActionList);
            xmlWriter.WriteAttributeString("ShelvedAttributes", shelvedAttributeIdList);
            xmlWriter.WriteAttributeString("ShelvedAttributeLocales", shelvedAttributeLocaleList);

            xmlWriter.WriteAttributeString("ImpactedRelationships", relationshipIdList);
            xmlWriter.WriteAttributeString("ImpactedRelationshipLocales", relationshipLocaleList);
            xmlWriter.WriteAttributeString("ShelvedRelationships", shelvedRelationshipsIdList);
            xmlWriter.WriteAttributeString("ShelvedRelationshipLocales", shelvedRelationshipsLocaleList);

            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("EntityActivityLogId", this.EntityActivityLogId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());

            #endregion write entity meta data for Full Xml

            //ImpactedEntity node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            entityXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityXml;
        }

        /// <summary>
        /// Get XML representation of impacted Entity object
        /// </summary>
        /// <param name="serializationType">Indicates type of serialization</param>
        /// <returns>XML representation of Entity object</returns>
        public override String ToXml(ObjectSerialization serializationType)
        {
            String impactedEntityXml = String.Empty;

            if (serializationType == ObjectSerialization.Full)
            {
                impactedEntityXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                String attributeIdList = String.Empty;
                String attributeLocaleIdList = String.Empty;
                String attributeLocaleList = String.Empty;
                String relationshipIdList = String.Empty;
                String relationshipLocaleIdList = String.Empty;
                String relationshipLocaleList = String.Empty;
                String shelvedAttributeIdList = String.Empty;
                String shelvedRelationshipsIdList = String.Empty;
                String shelvedAttributeLocaleList = String.Empty;
                String shelvedAttributeLocaleIdList = String.Empty;
                String shelvedRelationshipsLocaleList = String.Empty;
                String shelvedRelationshipsLocaleIdList = String.Empty;

                if (this.ImpactedAttributes != null && this.ImpactedAttributes.Count > 0)
                {
                    attributeIdList = ValueTypeHelper.JoinCollection(this.ImpactedAttributes, ",");
                }

                if (this.ImpactedAttributeLocales != null && this.ImpactedAttributeLocales.Count > 0)
                {
                    StringBuilder sbLocaleList = new StringBuilder();
                    StringBuilder sbLocaleIdList = new StringBuilder();

                    foreach (LocaleEnum locale in ImpactedAttributeLocales)
                    {
                        if (sbLocaleIdList.Length > 0)
                        {
                            sbLocaleIdList.Append(",");
                            sbLocaleList.Append(",");
                        }
                        sbLocaleList.Append(locale);
                        sbLocaleIdList.Append((Int32)locale);
                    }

                    attributeLocaleList = sbLocaleList.ToString();
                    attributeLocaleIdList = sbLocaleIdList.ToString();
                }

                if (this.ShelvedAttributes != null && this.ShelvedAttributes.Count > 0)
                {
                    shelvedAttributeIdList = ValueTypeHelper.JoinCollection(this.ShelvedAttributes, ",");
                }

                if (this.ShelvedAttributeLocales != null && this.ShelvedAttributeLocales.Count > 0)
                {
                    StringBuilder sbLocaleList = new StringBuilder();
                    StringBuilder sbLocaleIdList = new StringBuilder();

                    foreach (LocaleEnum locale in this.ShelvedAttributeLocales)
                    {
                        if (sbLocaleIdList.Length > 0)
                        {
                            sbLocaleIdList.Append(",");
                            sbLocaleList.Append(",");
                        }

                        sbLocaleList.Append(locale);
                        sbLocaleIdList.Append((Int32)locale);
                    }

                    shelvedAttributeLocaleList = sbLocaleList.ToString();
                    shelvedAttributeLocaleIdList = sbLocaleIdList.ToString();
                }

                if (this.ImpactedRelationships != null && this.ImpactedRelationships.Count > 0)
                {
                    relationshipIdList = ValueTypeHelper.JoinCollection(this.ImpactedRelationships, ",");
                }

                if (this.ImpactedRelationshipLocales != null && this.ImpactedRelationshipLocales.Count > 0)
                {
                    StringBuilder sbLocaleList = new StringBuilder();
                    StringBuilder sbLocaleIdList = new StringBuilder();

                    foreach (LocaleEnum locale in this.ImpactedRelationshipLocales.Where(l => l != LocaleEnum.UnKnown))
                    {
                        if (sbLocaleIdList.Length > 0)
                        {
                            sbLocaleIdList.Append(",");
                            sbLocaleList.Append(",");
                        }

                        sbLocaleList.Append(locale);
                        sbLocaleIdList.Append((Int32)locale);
                    }

                    relationshipLocaleList = sbLocaleList.ToString();
                    relationshipLocaleIdList = sbLocaleIdList.ToString(); ;
                }

                if (this.ShelvedRelationships != null && this.ShelvedRelationships.Count > 0)
                {
                    shelvedRelationshipsIdList = ValueTypeHelper.JoinCollection(this.ShelvedRelationships, ",");
                }

                if (this.ShelvedRelationshipLocales != null && this.ShelvedRelationshipLocales.Count > 0)
                {
                    StringBuilder sbLocaleList = new StringBuilder();
                    StringBuilder sbLocaleIdList = new StringBuilder();

                    foreach (LocaleEnum locale in ShelvedRelationshipLocales.Where(l => l != LocaleEnum.UnKnown))
                    {
                        if (sbLocaleIdList.Length > 0)
                        {
                            sbLocaleIdList.Append(",");
                            sbLocaleList.Append(",");
                        }

                        sbLocaleList.Append(locale);
                        sbLocaleIdList.Append((Int32)locale);
                    }

                    shelvedAttributeLocaleList = sbLocaleList.ToString();
                    shelvedAttributeLocaleIdList = sbLocaleIdList.ToString();
                }

                xmlWriter.WriteStartElement("ImpactedEntity");

                #region write impacted entity meta data for Xml

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                xmlWriter.WriteAttributeString("EntityName", this.EntityName);
                xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName);
                xmlWriter.WriteAttributeString("IsCacheDirty", this.IsCacheDirty.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("Context", this.Context);
                xmlWriter.WriteAttributeString("Priority", this.Priority.ToString());

                xmlWriter.WriteAttributeString("IsAttributeDenormRequired", this.IsEntityDenormRequired.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("ImpactedAttributes", attributeIdList);

                if (serializationType == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("ImpactedAttributeLocales", attributeLocaleIdList);
                }
                else
                {
                    xmlWriter.WriteAttributeString("ImpactedAttributeLocales", attributeLocaleList);
                }

                xmlWriter.WriteAttributeString("IsAttributeDenormInProcess", this.IsEntityDenormInProcess.ToString().ToLowerInvariant());
                xmlWriter.WriteAttributeString("ShelvedAttributes", shelvedAttributeIdList);

                if (serializationType == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("ShelvedAttributeLocales", shelvedAttributeLocaleIdList);
                }
                else
                {
                    xmlWriter.WriteAttributeString("ShelvedAttributeLocales", shelvedAttributeLocaleList);
                }

                xmlWriter.WriteAttributeString("ImpactedRelationships", relationshipIdList);
                if (serializationType == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("ImpactedRelationshipLocales", relationshipLocaleIdList);
                }
                else
                {
                    xmlWriter.WriteAttributeString("ImpactedRelationshipLocales", relationshipLocaleList);
                }
                
                xmlWriter.WriteAttributeString("ShelvedRelationships", shelvedRelationshipsIdList);
                if (serializationType == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("ShelvedRelationshipLocales", shelvedRelationshipsLocaleIdList);
                }
                else
                {
                    xmlWriter.WriteAttributeString("ShelvedRelationshipLocales", shelvedRelationshipsLocaleList);
                }

                xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                xmlWriter.WriteAttributeString("EntityActivityLogId", this.EntityActivityLogId.ToString());
                xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                #endregion write entity meta data for Xml

                //ImpactedEntity node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                impactedEntityXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return impactedEntityXml;
        }

        #endregion Methods
    }
}
