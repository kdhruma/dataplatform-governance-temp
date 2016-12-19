using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.Generic;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies EntityChangeContext which indicates what all information is to be loaded in Entity object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityChangeContext : ObjectBase, IEntityChangeContext
    {
        #region Fields

        #region Legacy Fields

        /// <summary>
        /// 
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// 
        /// </summary>
        private String _categoryName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private String _categoryPath = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isHierarchyChanged = false;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isExtensionsChanged = false;

        #endregion

        /// <summary>
        /// Indicates entity id for entity change context
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Indicates the parent id of an entity
        /// </summary>
        private Int64 _parentEntityId = -1;

        /// <summary>
        /// Indicates the parent extension id of an entity
        /// </summary>
        private Int64 _parentExtensionEntityId = -1;

        /// <summary>
        /// Indicates entity type id for entity change context.
        /// </summary>
        private Int32 _entityTypeId = -1;

        /// <summary>
        /// Indicates entity type name for entity change context.
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Indicates variant level for entity change context.
        /// </summary>
        private Int16 _variantLevel = -1;

        /// <summary>
        /// Indicates the action for entity change context.
        /// </summary>
        private ObjectAction _action = ObjectAction.Unknown;

        /// <summary>
        /// Indicates collection of locale change context including attribute change contexts.
        /// </summary>
        private LocaleChangeContextCollection _localeChangeContexts = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityChangeContext()
            : base()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public EntityChangeContext(String valuesAsXml)
        {
            LoadEntityChangeContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor with entity as input parameters
        /// </summary>
        /// <param name="entity">Indicates entity object</param>
        public EntityChangeContext(Entity entity)
        {
            RefreshEntityChangeContext(entity);
        }

        #endregion Constructors

        #region Properties

        #region Legacy Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
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
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
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
        /// This will be removed once uniquness is corrected for attribute model
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Collection<String> AttributeParentNameList
        {
            get;
            set;
        }

        /// <summary>
        /// This will be removed once uniquness is corrected for attribute model
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Collection<String> RelationshipAttributeParentNameList
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Gets attribute id list from entity change context
        /// </summary>
        public Collection<Int32> AttributeIdList
        {
            get
            {
                return this.LocaleChangeContexts.GetAttributeIdList();
            }
        }

        /// <summary>
        /// Gets attribute name list from entity change context
        /// </summary>
        public Collection<String> AttributeNameList
        {
            get
            {
                return this.LocaleChangeContexts.GetAttributeNameList();
            }
        }

        /// <summary>
        /// Gets attribute locale list from entity change context
        /// </summary>
        public Collection<LocaleEnum> AttributeLocaleList
        {
            get
            {
                return this.LocaleChangeContexts.GetAttributeLocaleList();
            }
        }

        /// <summary>
        /// Gets relationship id list from entity change context
        /// </summary>
        public Collection<Int64> RelationshipIdList
        {
            get
            {
                return this.LocaleChangeContexts.GetRelationshipIdList();
            }
        }

        /// <summary>
        /// Gets relationship type id list from entity change context
        /// </summary>
        public Collection<Int32> RelationshipTypeIdList
        {
            get
            {
                return this.LocaleChangeContexts.GetRelationshipTypeIdList();
            }
        }

        /// <summary>
        /// Gets relationship type name list from entity change context
        /// </summary>
        public Collection<String> RelationshipTypeNameList
        {
            get
            {
                return this.LocaleChangeContexts.GetRelationshipTypeNameList();
            }
        }

        /// <summary>
        /// Gets relationship attribute id list from entity change context
        /// </summary>
        public Collection<Int32> RelationshipAttributeIdList
        {
            get
            {
                return this.LocaleChangeContexts.GetRelationshipAttributeIdList();
            }
        }

        /// <summary>
        /// Gets relationship attribute name list from entity change context
        /// </summary>
        public Collection<String> RelationshipAttributeNameList
        {
            get
            {
                return this.LocaleChangeContexts.GetRelationshipAttributeNameList();
            }
        }

        /// <summary>
        /// Indicates locale list for relationships which are changed
        /// </summary>
        public Collection<LocaleEnum> RelationshipAttributeLocaleList
        {
            get
            {
                return this.LocaleChangeContexts.GetRelationshipAttributeLocaleList();
            }
        }

        /// <summary>
        /// Checks whether attribute object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        public Boolean IsAttributesChanged
        {
            get
            {
                return this.LocaleChangeContexts.HasAttributesChanged();
            }
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        public Boolean IsRelationshipsChanged
        {
            get
            {
                return this.LocaleChangeContexts.HasRelationshipsChanged();
            }
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Create
        /// </summary>
        public Boolean IsRelationshipsCreated
        {
            get
            {
                return this.LocaleChangeContexts.IsRelationshipsCreated();
            }
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Update
        /// </summary>
        public Boolean IsRelationshipsUpdated
        {
            get
            {
                return this.LocaleChangeContexts.IsRelationshipsUpdated();
            }
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Delete 
        /// </summary>
        public Boolean IsRelationshipsDeleted
        {
            get
            {
                return this.LocaleChangeContexts.IsRelationshipsDeleted();
            }
        }

        /// <summary>
        /// Checks whether relationship attributes object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        public Boolean IsRelationshipAttributesChanged
        {
            get
            {
                return this.LocaleChangeContexts.HasRelationshipAttributesChanged();
            }
        }

        /// <summary>
        /// Checks whether current object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Boolean IsHierarchyChanged
        {
            get
            {
                return this._isHierarchyChanged;
            }
            set
            {
                this._isHierarchyChanged = value;
            }
        }

        /// <summary>
        /// Checks whether current object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Boolean IsExtensionsChanged
        {
            get
            {
                return _isExtensionsChanged;
            }
            set
            {
                this._isExtensionsChanged = value;
            }
        }

        /// <summary>
        /// Specifies entity id based on EaaH change context.
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public Int64 EntityId
        {
            get
            {
                return this._entityId;
            }
            set
            {
                this._entityId = value;
            }
        }

        /// <summary>
        /// Indicates the parent id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
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
        /// Indicates the parent extension id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
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
        /// Specifies the entity type id based on EaaH change context.
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
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
        /// Specifies the entity type name based on EaaH change context.
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public String EntityTypeName
        {
            get
            {
                return this._entityTypeName;
            }
            set
            {
                this._entityTypeName = value;
            }
        }

        /// <summary>
        /// Specifies the variant level based on EaaH change context.
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public Int16 VariantLevel
        {
            get
            {
                return this._variantLevel;
            }
            set
            {
                this._variantLevel = value;
            }
        }

        /// <summary>
        /// Specifies the action based on EaaH change context.
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public ObjectAction Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        /// <summary>
        /// Specifies collection of locale change context 
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public LocaleChangeContextCollection LocaleChangeContexts
        {
            get
            {
                if (this._localeChangeContexts == null)
                {
                    this._localeChangeContexts = new LocaleChangeContextCollection();
                }

                return this._localeChangeContexts;
            }
            set
            {
                this._localeChangeContexts = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //EntityChangeContext node start
                    xmlWriter.WriteStartElement("EntityChangeContext");

                    #region write EntityChangeContext

                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("ParentEntityId", this.ParentEntityId.ToString());
                    xmlWriter.WriteAttributeString("ParentExtensionEntityId", this.ParentExtensionEntityId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
                    xmlWriter.WriteAttributeString("VariantLevel", this.VariantLevel.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion write EntityChangeContext

                    #region write locale change context including attribute change context xml

                    if (this._localeChangeContexts != null)
                    {
                        xmlWriter.WriteRaw(this.LocaleChangeContexts.ToXml());
                    }

                    #endregion write locale change context including attribute change context xml

                    //EntityChangeContext node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is EntityChangeContext)
            {
                EntityChangeContext objectToBeCompared = obj as EntityChangeContext;

                if (this.EntityId != objectToBeCompared.EntityId)
                {
                    return false;
                }
                if (this.ParentEntityId != objectToBeCompared.ParentEntityId)
                {
                    return false;
                }
                if (this.ParentExtensionEntityId != objectToBeCompared.ParentExtensionEntityId)
                {
                    return false;
                }
                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                {
                    return false;
                }
                if (this.EntityTypeName != objectToBeCompared.EntityTypeName)
                {
                    return false;
                }
                if (this.VariantLevel != objectToBeCompared.VariantLevel)
                {
                    return false;
                }
                if (this.Action != objectToBeCompared.Action)
                {
                    return false;
                }
                if (!this.LocaleChangeContexts.Equals(objectToBeCompared.LocaleChangeContexts))
                {
                    return false;
                }

                return true;
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

            hashCode = this.EntityId.GetHashCode() ^ this.ParentEntityId.GetHashCode() ^ this.ParentExtensionEntityId.GetHashCode() ^
                       this.EntityTypeId.GetHashCode() ^ this.EntityTypeName.GetHashCode() ^ this.VariantLevel.GetHashCode() ^ this.Action.GetHashCode() ^
                       this.LocaleChangeContexts.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Merges given entity Change Context into this object.
        /// </summary>
        /// <param name="deltaEntityChangeContext"></param>
        public void Merge(EntityChangeContext deltaEntityChangeContext)
        {
            if (deltaEntityChangeContext == null)
            {
                return;
            }

            this.Action = EntityFamilyChangeContext.GetMergedAction(this.Action, deltaEntityChangeContext.Action);

            this.LocaleChangeContexts.Merge(deltaEntityChangeContext.LocaleChangeContexts);
        }

        /// <summary>
        /// Gets the attribute change contexts
        /// </summary>
        /// <param name="attributes">Indicates collection of attribute object</param>
        /// <returns>Returns attribute change contexts</returns>
        public Dictionary<LocaleEnum, Dictionary<ObjectAction, AttributeChangeContext>> GetAttributeChangeContexts(AttributeCollection attributes)
        {
            Dictionary<LocaleEnum, Dictionary<ObjectAction, AttributeChangeContext>> attributeChangeConexts = null;

            if (attributes != null && attributes.Count > 0)
            {
                attributeChangeConexts = new Dictionary<LocaleEnum, Dictionary<ObjectAction, AttributeChangeContext>>();

                foreach (Attribute attribute in attributes)
                {
                    Int32 attributeId = attribute.Id;
                    String attributeName = attribute.Name;
                    ObjectAction objectAction = attribute.Action;
                    LocaleEnum attributeLocale = attribute.Locale;
                    AttributeChangeContext attributeChangeContext = null;

                    //If attribute action is set to read or ignore then skip those attributes to log into change context.
                    if (objectAction == ObjectAction.Read || objectAction == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    Dictionary<ObjectAction, AttributeChangeContext> attributeChangeContextBasedOnAction = null;
                    attributeChangeConexts.TryGetValue(attributeLocale, out attributeChangeContextBasedOnAction);
                    AttributeInfo attributeInfo = new AttributeInfo(attribute.Id, attribute.Name, attribute.AttributeParentId, attribute.AttributeParentName);

                    if (attributeChangeContextBasedOnAction == null)
                    {
                        attributeChangeContextBasedOnAction = new Dictionary<ObjectAction, AttributeChangeContext>();
                        attributeChangeContext = new AttributeChangeContext(attributeInfo, objectAction);

                        attributeChangeContextBasedOnAction.Add(objectAction, attributeChangeContext);
                        attributeChangeConexts.Add(attributeLocale, attributeChangeContextBasedOnAction);
                    }
                    else
                    {
                        attributeChangeContextBasedOnAction.TryGetValue(objectAction, out attributeChangeContext);

                        if (attributeChangeContext == null)
                        {
                            attributeChangeContext = new AttributeChangeContext(attributeInfo, objectAction);
                            attributeChangeContextBasedOnAction.Add(objectAction, attributeChangeContext);
                        }
                        else
                        {
                            attributeChangeContext.AttributeInfoCollection.Add(attributeInfo);
                        }
                    }
                }
            }

            return attributeChangeConexts;
        }

        #region Locale Change Context related methods

        /// <summary>
        /// Gets the locale change contexts of an entity.
        /// </summary>
        /// <returns>Locale change contexts of an entity.</returns>
        public ILocaleChangeContextCollection GetLocaleChangeContexts()
        {
            if (this._localeChangeContexts == null)
            {
                return null;
            }

            return (ILocaleChangeContextCollection)this.LocaleChangeContexts;
        }

        /// <summary>
        /// Sets the locale change contexts of an entity.
        /// </summary>
        /// <param name="iLocaleChangeContexts">Indicates the locale change context to be set</param>
        public void SetLocaleChangeContexts(ILocaleChangeContextCollection iLocaleChangeContexts)
        {
            this.LocaleChangeContexts = (LocaleChangeContextCollection)iLocaleChangeContexts;
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityChangeContext")
                    {
                        #region Read EntityContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("EntityId"))
                            {
                                this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityId);
                            }
                            if (reader.MoveToAttribute("ParentEntityId"))
                            {
                                this.ParentEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._parentEntityId);
                            }
                            if (reader.MoveToAttribute("ParentExtensionEntityId"))
                            {
                                this.ParentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._parentExtensionEntityId);
                            }
                            if (reader.MoveToAttribute("EntityTypeId"))
                            {
                                this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._entityTypeId);
                            }
                            if (reader.MoveToAttribute("EntityTypeName"))
                            {
                                this.EntityTypeName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("VariantLevel"))
                            {
                                this.VariantLevel = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), this._variantLevel);
                            }
                            if (reader.MoveToAttribute("Action"))
                            {
                                ObjectAction objectAction = ObjectAction.Unknown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out objectAction);
                                this.Action = objectAction;
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleChangeContexts")
                    {
                        #region Read LocaleChangeContexts

                        String localeChangeContextsXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(localeChangeContextsXml))
                        {
                            LocaleChangeContextCollection localeChangeContexts = new LocaleChangeContextCollection(localeChangeContextsXml);

                            if (localeChangeContexts != null && localeChangeContexts.Count > 0)
                            {
                                this.LocaleChangeContexts = localeChangeContexts;
                            }
                        }

                        #endregion Read LocaleChangeContexts
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
        /// Refresh the entity change context for given entity object
        /// </summary>
        /// <param name="entity">Indicates entity object</param>
        private void RefreshEntityChangeContext(Entity entity)
        {
            if (entity != null)
            {
                LocaleChangeContextCollection localeChangeContexts = new LocaleChangeContextCollection();

                InitializeMembers(entity.Id, entity.ParentEntityId, entity.ParentExtensionEntityId, entity.EntityTypeId, entity.EntityTypeName, entity.HierarchyLevel, entity.Action);

                #region Populate Attribute Change Contexts Per Locale

                Dictionary<LocaleEnum, Dictionary<ObjectAction, AttributeChangeContext>> attributesChangeConexts = GetAttributeChangeContexts(entity.Attributes);

                if (attributesChangeConexts != null && attributesChangeConexts.Count > 0)
                {
                    var attrsEnumerator = attributesChangeConexts.GetEnumerator();

                    while (attrsEnumerator.MoveNext())
                    {
                        LocaleChangeContext localeChangeContext = new LocaleChangeContext();
                        localeChangeContext.DataLocale = attrsEnumerator.Current.Key;

                        var attrsChangeContextBasedOnActionEnumerator = attrsEnumerator.Current.Value.GetEnumerator();

                        while (attrsChangeContextBasedOnActionEnumerator.MoveNext())
                        {
                            localeChangeContext.AttributeChangeContexts.Add(attrsChangeContextBasedOnActionEnumerator.Current.Value);
                        }

                        localeChangeContexts.Add(localeChangeContext);
                    }
                }

                #endregion

                #region Populate Relationship Change Context Per Locale

                PopulateRelationshipChangeContext(entity.Relationships, localeChangeContexts);

                #endregion

                this.SetLocaleChangeContexts(localeChangeContexts);

                ExtensionRelationshipCollection extensionRelationships = entity.ExtensionRelationships;
                HierarchyRelationshipCollection hierarchyRelationships = entity.HierarchyRelationships;

                if (extensionRelationships != null && extensionRelationships.Count > 0)
                {
                    this._isExtensionsChanged = extensionRelationships.HasObjectChanged();
                }

                if (hierarchyRelationships != null && hierarchyRelationships.Count > 0)
                {
                    this._isHierarchyChanged = hierarchyRelationships.HasObjectChanged();
                }
            }
        }

        /// <summary>
        /// Populates relationship change context for given relationships
        /// </summary>
        /// <param name="relationships">Indicates collection of relationship</param>
        /// <param name="localeChangeContexts">Indicates collection of locale change context</param>
        private void PopulateRelationshipChangeContext(RelationshipCollection relationships, LocaleChangeContextCollection localeChangeContexts)
        {
            foreach (Relationship relationship in relationships)
            {
                //If relationship action is set to read or ignore then skip those relationships to log into change context.
                if (relationship.Action == ObjectAction.Read || relationship.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                Int64 relationshipId = relationship.Id;
                LocaleEnum relLocale = relationship.Locale;

                LocaleChangeContext localeChangeContext = localeChangeContexts.Get(relLocale);

                if (localeChangeContext == null)
                {
                    localeChangeContext = new LocaleChangeContext();
                    localeChangeContext.DataLocale = relLocale;
                    localeChangeContexts.Add(localeChangeContext);
                }

                RelationshipChangeContext relationshipChangeContext = new RelationshipChangeContext(relationshipId, relationship.FromEntityId, relationship.RelatedEntityId,
                                                                      relationship.RelationshipTypeId, relationship.RelationshipTypeName, relationship.Action);

                localeChangeContext.RelationshipChangeContexts.Add(relationshipChangeContext);

                if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                {
                    Dictionary<LocaleEnum, Dictionary<ObjectAction, AttributeChangeContext>> relationshipAttributesChangeConexts = GetAttributeChangeContexts(relationship.RelationshipAttributes);

                    if (relationshipAttributesChangeConexts != null && relationshipAttributesChangeConexts.Count > 0)
                    {
                        var relAttrsEnumerator = relationshipAttributesChangeConexts.GetEnumerator();

                        while (relAttrsEnumerator.MoveNext())
                        {
                            LocaleEnum dataLocale = relAttrsEnumerator.Current.Key;
                            localeChangeContext = localeChangeContexts.Get(dataLocale);

                            if (localeChangeContext == null)
                            {
                                localeChangeContext = new LocaleChangeContext();
                                localeChangeContext.DataLocale = dataLocale;

                                localeChangeContexts.Add(localeChangeContext);
                            }

                            relationshipChangeContext = localeChangeContext.RelationshipChangeContexts.Get(relationshipId);

                            if (relationshipChangeContext == null)
                            {
                                relationshipChangeContext = new RelationshipChangeContext(relationshipId, relationship.RelatedEntityId, relationship.RelatedEntityId,
                                relationship.RelationshipTypeId, relationship.RelationshipTypeName, relationship.Action);

                                localeChangeContext.RelationshipChangeContexts.Add(relationshipChangeContext);
                            }

                            var relAttrsChangeContextBasedOnActionEnumerator = relAttrsEnumerator.Current.Value.GetEnumerator();

                            while (relAttrsChangeContextBasedOnActionEnumerator.MoveNext())
                            {
                                relationshipChangeContext.AttributeChangeContexts.Add(relAttrsChangeContextBasedOnActionEnumerator.Current.Value);
                            }
                        }
                    }
                }

                #region Prepare change context for child relationships

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    PopulateRelationshipChangeContext(relationship.RelationshipCollection, localeChangeContexts);
                }

                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="parentEntityId"></param>
        /// <param name="parentExtensionEntityId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="variantsLevel"></param>
        /// <param name="action"></param>
        private void InitializeMembers(Int64 entityId, Int64 parentEntityId, Int64 parentExtensionEntityId, Int32 entityTypeId, String entityTypeName, Int16 variantsLevel, ObjectAction action)
        {
            this._entityId = entityId;
            this._parentEntityId = parentEntityId;
            this._parentExtensionEntityId = parentExtensionEntityId;
            this._entityTypeId = entityTypeId;
            this._entityTypeName = entityTypeName;
            this._variantLevel = variantsLevel;
            this._action = action;
        }

        #endregion Private Methods

        #endregion Methods
    }
}