using ProtoBuf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the entity family change context of entities
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityFamilyChangeContext : ObjectBase, IEntityFamilyChangeContext
    {
        #region Fields

        /// <summary>
        /// Indicates entity family id for entity family change context
        /// </summary>
        private Int64 _entityFamilyId = -1;

        /// <summary>
        /// Indicates entity global family id for entity family change context
        /// </summary>
        private Int64 _entityGlobalFamilyId = -1;

        /// <summary>
        /// Indicates organization id of entity family id
        /// </summary>
        private Int32 _organizationId = -1;

        /// <summary>
        /// Indicates container id of entity family id
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Indicates family activity list of an entity.
        /// </summary>
        private EntityActivityList _entityActivityList = EntityActivityList.UnKnown;

        /// <summary>
        /// Indicates whether entity family change context record for master collaboration or not
        /// </summary>
        private Boolean _isMasterCollaborationRecord = false;

        /// <summary>
        /// Field denoting the business rule name
        /// </summary>
        private String _businessRuleName = String.Empty;

        /// <summary>
        /// Field denoting business rule context name
        /// </summary>
        private String _businessRuleContextName = String.Empty;

        /// <summary>
        /// Indicates variants change contexts for entity family  change context
        /// </summary>
        private VariantsChangeContext _variantsChangeContext = null;

        /// <summary>
        /// Indicates extension change contexts collection for entity family  change context
        /// </summary>
        private ExtensionChangeContextCollection _extensionChangeContexts = null;

        /// <summary>
        /// Indicates workflow change context for an entity
        /// </summary>
        private WorkflowChangeContext _workflowChangeContext = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public EntityFamilyChangeContext()
            : base()
        {
        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public EntityFamilyChangeContext(String valuesAsXml)
        {
            LoadEntityFamilyChangeContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor with entity family id and entity global family id of an entity family change context as input parameters
        /// </summary>
        /// <param name="entityFamilyId">Indicates entity family id for entity family context</param>
        /// <param name="entityGlobalFamilyId">Indicates entity global family id for entity family change context</param>
        /// <param name="organizationId">Indicates organization id for an entity family</param>
        /// <param name="containerId">Indicates container id for an entity family</param>
        public EntityFamilyChangeContext(Int64 entityFamilyId, Int64 entityGlobalFamilyId, Int32 organizationId, Int32 containerId)
        {
            this._entityFamilyId = entityFamilyId;
            this._entityGlobalFamilyId = entityGlobalFamilyId;
            this._organizationId = organizationId;
            this._containerId = containerId;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies entity family id for entity family change context
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int64 EntityFamilyId
        {
            get
            {
                return this._entityFamilyId;
            }
            set
            {
                this._entityFamilyId = value;
            }
        }

        /// <summary>
        /// Specifies entity global family id for entity family change context
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 EntityGlobalFamilyId
        {
            get
            {
                return this._entityGlobalFamilyId;
            }
            set
            {
                this._entityGlobalFamilyId = value;
            }
        }

        /// <summary>
        /// Specifies organization id for entity family change context
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
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
        /// Specifies container id for entity family change context
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
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
        /// Specifies family activity list of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public EntityActivityList EntityActivityList
        {
            get
            {
                return this._entityActivityList;
            }
            set
            {
                this._entityActivityList = value;
            }
        }

        /// <summary>
        /// Indicates whether entity family queue record for master collaboration or not
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Boolean IsMasterCollaborationRecord
        {
            get
            {
                return this._isMasterCollaborationRecord;
            }
            set
            {
                this._isMasterCollaborationRecord = value;
            }
        }

        /// <summary>
        /// Property indicating the name of the business rule.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public String BusinessRuleName
        {
            get
            {
                return _businessRuleName;
            }
            set
            {
                _businessRuleName = value;
            }
        }

        /// <summary>
        /// Property indicating the name of the business rule context.
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public String BusinessRuleContextName
        {
            get
            {
                return _businessRuleContextName;
            }
            set
            {
                _businessRuleContextName = value;
            }
        }

        /// <summary>
        /// Specifies variants change context for entity family change context
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public VariantsChangeContext VariantsChangeContext
        {
            get
            {
                if (this._variantsChangeContext == null)
                {
                    this._variantsChangeContext = new VariantsChangeContext();
                }

                return this._variantsChangeContext;
            }
            set
            {
                this._variantsChangeContext = value;
            }
        }

        /// <summary>
        /// Specifies collection of extension change context.
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public ExtensionChangeContextCollection ExtensionChangeContexts
        {
            get
            {
                if (this._extensionChangeContexts == null)
                {
                    this._extensionChangeContexts = new ExtensionChangeContextCollection();
                }

                return this._extensionChangeContexts;
            }
            set
            {
                this._extensionChangeContexts = value;
            }
        }

        /// <summary>
        /// Specifies workflow change context for an entity
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public WorkflowChangeContext WorkflowChangeContext
        {
            get
            {
                if (this._workflowChangeContext == null)
                {
                    this._workflowChangeContext = new WorkflowChangeContext();
                }

                return this._workflowChangeContext;
            }
            set
            {
                this._workflowChangeContext = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        #region ToXml Methods

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
                    //EntityFamilyChangeContext node start
                    xmlWriter.WriteStartElement("EntityFamilyChangeContext");
                    xmlWriter.WriteAttributeString("EntityFamilyId", this.EntityFamilyId.ToString());
                    xmlWriter.WriteAttributeString("EntityGlobalFamilyId", this.EntityGlobalFamilyId.ToString());
                    xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("EntityActivityList", this.EntityActivityList.ToString());
                    xmlWriter.WriteAttributeString("BusinessRuleName", this.BusinessRuleName);
                    xmlWriter.WriteAttributeString("BusinessRuleContextName", this.BusinessRuleContextName);

                    #region write variants change context

                    if (this._variantsChangeContext != null)
                    {
                        xmlWriter.WriteRaw(this.VariantsChangeContext.ToXml());
                    }

                    #endregion write variants change contex

                    #region write extension change context

                    if (this._extensionChangeContexts != null)
                    {
                        xmlWriter.WriteRaw(this.ExtensionChangeContexts.ToXml());
                    }

                    #endregion write extension change context

                    #region write workflow change context

                    if (this._workflowChangeContext != null)
                    {
                        xmlWriter.WriteRaw(this.WorkflowChangeContext.ToXml());
                    }

                    #endregion write extension change context

                    //EntityFamilyChangeContext node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        #endregion ToXml Methods

        #region Variants Change Context related methods

        /// <summary>
        /// Gets the variants change context of an entity family
        /// </summary>
        /// <returns>Variants change context of an entity family</returns>
        public IVariantsChangeContext GetVariantsChangeContext()
        {
            if (this._variantsChangeContext == null)
            {
                return null;
            }

            return (IVariantsChangeContext)this.VariantsChangeContext;
        }

        /// <summary>
        /// Sets the variants change context of an entity family
        /// </summary>
        /// <param name="iVariantsChangeContext">Indicates the variants change context to be set</param>
        public void SetVariantsChangeContext(IVariantsChangeContext iVariantsChangeContext)
        {
            this.VariantsChangeContext = (VariantsChangeContext)iVariantsChangeContext;
        }

        #endregion

        #region Extension Change Contexts related methods

        /// <summary>
        /// Gets the extension change contexts of an entity family
        /// </summary>
        /// <returns>Gets the extension change contexts of an entity family</returns>
        public IExtensionChangeContextCollection GetExtensionChangeContexts()
        {
            if (this._extensionChangeContexts == null)
            {
                return null;
            }

            return (IExtensionChangeContextCollection)this.ExtensionChangeContexts;
        }

        /// <summary>
        /// Sets the extension change context of an entity family
        /// </summary>
        /// <param name="iExtensionChangeContexts">Indicates the extension change context to be set</param>
        public void SetExtensionChangeContexts(IExtensionChangeContextCollection iExtensionChangeContexts)
        {
            this.ExtensionChangeContexts = (ExtensionChangeContextCollection)iExtensionChangeContexts;
        }

        #endregion

        #region Workflow Change Context related methods

        /// <summary>
        /// Gets the workflow change contexts of an entity.
        /// </summary>
        /// <returns>Gets the workflow change contexts of an entity</returns>
        public IWorkflowChangeContext GetWorkflowChangeContext()
        {
            if (this._workflowChangeContext == null)
            {
                return null;
            }

            return (IWorkflowChangeContext)this._workflowChangeContext;
        }

        /// <summary>
        /// Sets the workflow change context of an entity
        /// </summary>
        /// <param name="iWorkflowChangeContext">Indicates the workflow change context to be set</param>
        public void SetWorkflowChangeContext(IWorkflowChangeContext iWorkflowChangeContext)
        {
            this.WorkflowChangeContext = (WorkflowChangeContext)iWorkflowChangeContext;
        }

        #endregion

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is EntityFamilyChangeContext)
            {
                EntityFamilyChangeContext objectToBeCompared = obj as EntityFamilyChangeContext;

                if (this.EntityFamilyId != objectToBeCompared.EntityFamilyId)
                {
                    return false;
                }
                if (this.EntityGlobalFamilyId != objectToBeCompared.EntityGlobalFamilyId)
                {
                    return false;
                }
                if (this.OrganizationId != objectToBeCompared.OrganizationId)
                {
                    return false;
                }
                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }
                if (this.EntityActivityList != objectToBeCompared.EntityActivityList)
                {
                    return false;
                }
                if (this.BusinessRuleName != objectToBeCompared.BusinessRuleName)
                {
                    return false;
                }
                if (this.BusinessRuleContextName != objectToBeCompared.BusinessRuleContextName)
                {
                    return false;
                }
                if (!this.VariantsChangeContext.Equals(objectToBeCompared.VariantsChangeContext))
                {
                    return false;
                }
                if (!this.ExtensionChangeContexts.Equals(objectToBeCompared.ExtensionChangeContexts))
                {
                    return false;
                }
                if (!this.WorkflowChangeContext.Equals(objectToBeCompared.WorkflowChangeContext))
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

            hashCode = this.EntityFamilyId.GetHashCode() ^ this.EntityGlobalFamilyId.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.ContainerId.GetHashCode() ^
                       this.EntityActivityList.GetHashCode() ^ this.BusinessRuleName.GetHashCode() ^ this.BusinessRuleContextName.GetHashCode() ^
                       this.VariantsChangeContext.GetHashCode() ^ this.ExtensionChangeContexts.GetHashCode() ^ this.WorkflowChangeContext.GetHashCode();

            return hashCode;
        }

        #region Merge related Methods

        /// <summary>
        /// Delta Merge of entity family change context
        /// </summary>
        /// <param name="deltaEntityFamilyChangeContext">Indicates entity family context needs to be merged</param>
        public void Merge(EntityFamilyChangeContext deltaEntityFamilyChangeContext)
        {
            if (String.IsNullOrWhiteSpace(this.BusinessRuleName))
            {
                this.BusinessRuleName = deltaEntityFamilyChangeContext.BusinessRuleName;
            }
            if (String.IsNullOrWhiteSpace(this.BusinessRuleContextName))
            {
                this.BusinessRuleName = deltaEntityFamilyChangeContext.BusinessRuleContextName;
            }

            #region Merge Variant Change Context

            this.VariantsChangeContext.Merge(deltaEntityFamilyChangeContext.VariantsChangeContext);

            #endregion Merge Variant Change Context

            #region Merge Extension Change Context

            this.ExtensionChangeContexts.Merge(deltaEntityFamilyChangeContext.ExtensionChangeContexts);

            #endregion Merge Extension Change Context
        }

        /// <summary>
        /// Gets the merged action based on given original action and current action
        /// </summary>
        /// <param name="originalAction">Indicates original action</param>
        /// <param name="currentAction">Indicates current action</param>
        /// <returns>Returns merged action for given original and current action</returns>
        internal static ObjectAction GetMergedAction(ObjectAction originalAction, ObjectAction currentAction)
        {
            ObjectAction mergedAction = ObjectAction.Unknown;

            if (originalAction == ObjectAction.Create &&
               (currentAction == ObjectAction.Update || currentAction == ObjectAction.Reclassify || currentAction == ObjectAction.Rename))
            {
                mergedAction = ObjectAction.Create;
            }
            else if (originalAction == ObjectAction.Create && currentAction == ObjectAction.Delete)
            {
                mergedAction = ObjectAction.Delete;
            }
            else if ((originalAction == ObjectAction.Reclassify || originalAction == ObjectAction.Rename) &&
                    currentAction == ObjectAction.Update)
            {
                mergedAction = originalAction;
            }
            else if (originalAction == ObjectAction.Update &&
                (currentAction == ObjectAction.Update || currentAction == ObjectAction.Reclassify || currentAction == ObjectAction.Rename))
            {
                mergedAction = currentAction;
            }
            else if ((originalAction == ObjectAction.Update || originalAction == ObjectAction.Reclassify || originalAction == ObjectAction.Rename) &&
                    currentAction == ObjectAction.Delete)
            {
                mergedAction = ObjectAction.Delete;
            }
            else if (originalAction == ObjectAction.Delete && currentAction == ObjectAction.Create)
            {
                mergedAction = ObjectAction.Create;
            }

            return mergedAction;
        }

        #endregion Merge related Methods

        #region Helper Methods

        /// <summary>
        /// Gets entity context based on entity change contexts
        /// </summary>
        /// <returns>Returns entity change context based on family change context</returns>
        public EntityContext GetEntityContext()
        {
            EntityContext entityContext = new EntityContext();

            PopulateEntityContext(this._variantsChangeContext, entityContext);

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                foreach (ExtensionChangeContext extensionChangeContext in this._extensionChangeContexts)
                {
                    PopulateEntityContext(extensionChangeContext.VariantsChangeContext, entityContext);
                }
            }

            return entityContext;
        }
        
        /// <summary>
        /// Gets the attribute id list from entity family change contexts
        /// </summary>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList()
        {
            Collection<Int32> attributeIdList = new Collection<Int32>();

            if (this._variantsChangeContext != null)
            {
                attributeIdList.AddRange<Int32>(this._variantsChangeContext.GetAttributeIdList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                attributeIdList.AddRange<Int32>(this._extensionChangeContexts.GetAttributeIdList());
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute locale list from entity family change contexts
        /// </summary>
        /// <returns>Returns attribute locale list</returns>
        public Collection<LocaleEnum> GetAttributeLocaleList()
        {
            Collection<LocaleEnum> attributeLocaleList = new Collection<LocaleEnum>();

            if (this._variantsChangeContext != null)
            {
                attributeLocaleList.AddRange<LocaleEnum>(this._variantsChangeContext.GetAttributeLocaleList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                attributeLocaleList.AddRange<LocaleEnum>(this._extensionChangeContexts.GetAttributeLocaleList());
            }

            return attributeLocaleList;
        }

        /// <summary>
        /// Gets the attribute name list from entity family change contexts
        /// </summary>
        /// <returns>Returns attribute name list</returns>
        public Collection<String> GetAttributeNameList()
        {
            Collection<String> attributeNameList = new Collection<String>();

            if (this._variantsChangeContext != null)
            {
                attributeNameList.AddRange<String>(this._variantsChangeContext.GetAttributeNameList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                attributeNameList.AddRange<String>(this._extensionChangeContexts.GetAttributeNameList());
            }

            return attributeNameList;
        }

        /// <summary>
        /// Gets the relationship type id list from entity family change contexts
        /// </summary>
        /// <returns>Returns relationship type id list</returns>
        public Collection<Int32> GetRelationshipTypeIdList()
        {
            Collection<Int32> relationshipTypeIdList = new Collection<Int32>();

            if (this._variantsChangeContext != null)
            {
                relationshipTypeIdList.AddRange<Int32>(this._variantsChangeContext.GetRelationshipTypeIdList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                relationshipTypeIdList.AddRange<Int32>(this._extensionChangeContexts.GetRelationshipTypeIdList());
            }

            return relationshipTypeIdList;
        }

        /// <summary>
        /// Gets the relationship type name list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship type name list</returns>
        public Collection<String> GetRelationshipTypeNameList()
        {
            Collection<String> relationshipTypeNameList = new Collection<String>();

            if (this._variantsChangeContext != null)
            {
                relationshipTypeNameList.AddRange<String>(this._variantsChangeContext.GetRelationshipTypeNameList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                relationshipTypeNameList.AddRange<String>(this._extensionChangeContexts.GetRelationshipTypeNameList());
            }

            return relationshipTypeNameList;
        }

        /// <summary>
        /// Gets the relationship attribute id list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute id list</returns>
        public Collection<Int32> GetRelationshipAttributeIdList()
        {
            Collection<Int32> relationshipAttributeIdList = new Collection<Int32>();

            if (this._variantsChangeContext != null)
            {
                relationshipAttributeIdList.AddRange<Int32>(this._variantsChangeContext.GetRelationshipAttributeIdList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                relationshipAttributeIdList.AddRange<Int32>(this._extensionChangeContexts.GetRelationshipAttributeIdList());
            }

            return relationshipAttributeIdList;
        }

        /// <summary>
        /// Gets the relationship attribute locale list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute locale list</returns>
        public Collection<LocaleEnum> GetRelationshipAttributeLocaleList()
        {
            Collection<LocaleEnum> relationshipAttributeLocaleList = new Collection<LocaleEnum>();

            if (this._variantsChangeContext != null)
            {
                relationshipAttributeLocaleList.AddRange<LocaleEnum>(this._variantsChangeContext.GetRelationshipAttributeLocaleList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                relationshipAttributeLocaleList.AddRange<LocaleEnum>(this._extensionChangeContexts.GetRelationshipAttributeLocaleList());
            }

            return relationshipAttributeLocaleList;
        }

        /// <summary>
        /// Gets the relationship attribute name list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute name list</returns>
        public Collection<String> GetRelationshipAttributeNameList()
        {
            Collection<String> relationshipAttributeNameList = new Collection<String>();

            if (this._variantsChangeContext != null)
            {
                relationshipAttributeNameList.AddRange<String>(this._variantsChangeContext.GetRelationshipAttributeNameList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                relationshipAttributeNameList.AddRange<String>(this._extensionChangeContexts.GetRelationshipAttributeNameList());
            }

            return relationshipAttributeNameList;
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasRelationshipsChanged()
        {
            Boolean hasRelationshipsChanged = false;

            if (this._variantsChangeContext != null)
            {
                hasRelationshipsChanged = this._variantsChangeContext.HasRelationshipsChanged();
            }

            if (!hasRelationshipsChanged && this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                hasRelationshipsChanged = this._extensionChangeContexts.HasRelationshipsChanged();
            }

            return hasRelationshipsChanged;
        }

        /// <summary>
        /// Checks whether any entity change context action is Create
        /// </summary>
        /// <returns>Return true if object is having Create</returns>
        public Boolean HasEntitiesCreated()
        {
            Boolean hasEntitiesCreated = false;

            if (this._variantsChangeContext != null)
            {
                hasEntitiesCreated = this._variantsChangeContext.HasEntitiesCreated();
            }

            if (!hasEntitiesCreated && this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                hasEntitiesCreated = this._extensionChangeContexts.HasEntitiesCreated();
            } 

            return hasEntitiesCreated;
        }

        /// <summary>
        /// Gets entity id list based on given object action.
        /// </summary>
        /// <param name="objectAction">Indicates object action</param>
        /// <returns>Returns collection of entity id list for given object action</returns>
        public Collection<Int64> GetEntityIdList(ObjectAction objectAction)
        {
            return GetEntityIdList(new Collection<ObjectAction>() { objectAction });
        }

        /// <summary>
        /// Gets entity id list based on given object action.
        /// </summary>
        /// <param name="objectActions">Indicates collection of object action</param>
        /// <returns>Returns collection of entity id list for given object action</returns>
        public Collection<Int64> GetEntityIdList(Collection<ObjectAction> objectActions)
        {
            Collection<Int64> entityIdList = new Collection<Int64>();

            if (this._variantsChangeContext != null)
            {
                entityIdList.AddRange<Int64>(this._variantsChangeContext.GetEntityIdList(objectActions));
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                entityIdList.AddRange<Int64>(this._extensionChangeContexts.GetEntityIdList(objectActions));
            }

            return entityIdList;
        }

        /// <summary>
        /// Gets the locale list from entity family change context
        /// </summary>
        /// <returns>Returns combined locale list of attribute and relationship change context</returns>
        public Collection<LocaleEnum> GetLocaleList()
        {
            Collection<LocaleEnum> localeList = new Collection<LocaleEnum>();

            if (this._variantsChangeContext != null)
            {
                localeList.AddRange<LocaleEnum>(this._variantsChangeContext.GetAttributeLocaleList());
                localeList.AddRange<LocaleEnum>(this._variantsChangeContext.GetRelationshipAttributeLocaleList());
            }

            if (this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                localeList.AddRange<LocaleEnum>(this._extensionChangeContexts.GetAttributeLocaleList());
                localeList.AddRange<LocaleEnum>(this._extensionChangeContexts.GetRelationshipAttributeLocaleList());
            }

            return localeList;
        }

        /// <summary>
        /// Get entity change context for given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity change context to be fetched</param>
        /// <returns>Returns entity change context for given entity id</returns>
        public EntityChangeContext GetEntityChangeContext(Int64 entityId)
        {
            EntityChangeContext entityChangeContext = null;

            if (this._variantsChangeContext != null)
            {
                entityChangeContext = this._variantsChangeContext.EntityChangeContexts.GetByEntityId(entityId);
            }

            if (entityChangeContext == null && this._extensionChangeContexts != null && this._extensionChangeContexts.Count > 0)
            {
                foreach (ExtensionChangeContext extensionChangeConext in this._extensionChangeContexts)
                {
                    entityChangeContext = extensionChangeConext.VariantsChangeContext.EntityChangeContexts.GetByEntityId(entityId);

                    if (entityChangeContext != null)
                    {
                        break;
                    }
                }
            }

            return entityChangeContext;
        }

        #endregion Helper Methods

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityFamilyChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityFamilyChangeContext")
                    {
                        #region Read EntityFamilyChangeContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("EntityFamilyId"))
                            {
                                this._entityFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityFamilyId);
                            }
                            if (reader.MoveToAttribute("EntityGlobalFamilyId"))
                            {
                                this._entityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityGlobalFamilyId);
                            }
                            if (reader.MoveToAttribute("OrganizationId"))
                            {
                                this._organizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._organizationId);
                            }
                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this._containerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }
                            if (reader.MoveToAttribute("EntityActivityList"))
                            {
                                EntityActivityList entityActivityList = EntityActivityList.UnKnown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out entityActivityList);
                                this._entityActivityList = entityActivityList;
                            }
                            if (reader.MoveToAttribute("BusinessRuleName"))
                            {
                                this._businessRuleName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("BusinessRuleContextName"))
                            {
                                this._businessRuleContextName = reader.ReadContentAsString();
                            }

                            reader.Read();
                        }

                        #endregion Read EntityFamilyChangeContext
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "VariantsChangeContext")
                    {
                        #region Read VariantsChangeContext

                        String variantsChangeContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(variantsChangeContextXml))
                        {
                            this._variantsChangeContext = new VariantsChangeContext(variantsChangeContextXml);
                        }

                        #endregion Read VariantsChangeContext
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtensionChangeContexts")
                    {
                        #region Read ExtensionChangeContexts

                        String extensionChangeContextsXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(extensionChangeContextsXml))
                        {
                            ExtensionChangeContextCollection extensionChangeContexts = new ExtensionChangeContextCollection(extensionChangeContextsXml);

                            if (extensionChangeContexts != null && extensionChangeContexts.Count > 0)
                            {
                                this.ExtensionChangeContexts = extensionChangeContexts;
                            }
                        }

                        #endregion Read ExtensionChangeContexts
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowChangeContext")
                    {
                        #region Read WorkflowChangeContext

                        String workflowChangeContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(workflowChangeContextXml))
                        {
                            this._workflowChangeContext = new WorkflowChangeContext(workflowChangeContextXml);
                        }

                        #endregion Read WorkflowChangeContext
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
        /// <param name="variantsChangeContext"></param>
        /// <param name="entityContext"></param>
        private void PopulateEntityContext(VariantsChangeContext variantsChangeContext, EntityContext entityContext)
        {
            if (variantsChangeContext != null)
            {
                EntityChangeContextCollection entityChangeContexts = variantsChangeContext.EntityChangeContexts;

                if (entityChangeContexts.Count > 0)
                {
                    foreach (EntityChangeContext entityChangeContext in entityChangeContexts)
                    {
                        entityContext.AttributeIdList.AddRange<Int32>(entityChangeContext.AttributeIdList);
                        entityContext.DataLocales.AddRange<LocaleEnum>(entityChangeContext.AttributeLocaleList);

                        entityContext.RelationshipContext.RelationshipTypeIdList.AddRange<Int32>(entityChangeContext.RelationshipTypeIdList);
                        entityContext.RelationshipContext.DataLocales.AddRange<LocaleEnum>(entityChangeContext.RelationshipAttributeLocaleList);
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}