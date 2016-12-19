using ProtoBuf;
using System;
using System.Collections.Generic;
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
    /// Specifies collection of change context of an entity.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityChangeContextCollection : InterfaceContractCollection<IEntityChangeContext, EntityChangeContext>, IEntityChangeContextCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the attribute change context Collection
        /// </summary>
        public EntityChangeContextCollection()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityChangeContextCollection(String valuesAsXml)
        {
            LoadEntityChangeContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="entityChangeContextList">List of entity change contexts</param>
        public EntityChangeContextCollection(IList<EntityChangeContext> entityChangeContextList)
        {
            if (entityChangeContextList != null)
            {
                this._items = new Collection<EntityChangeContext>(entityChangeContextList);
            }
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EaaHChangeContext Collection
        /// </summary>
        /// <returns>Xml representation of EaaHChangeContext Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //EntityChangeContextCollection node start
                    xmlWriter.WriteStartElement("EntityChangeContexts");

                    #region Write EaaHChangeContextCollection

                    if (_items != null)
                    {
                        foreach (EntityChangeContext entityChangeContext in this._items)
                        {
                            xmlWriter.WriteRaw(entityChangeContext.ToXml());
                        }
                    }

                    #endregion

                    //EntityChangeContextCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Delta Merge of entity change contexts
        /// </summary>
        /// <param name="deltaEntityChangeContexts">Indicates entity change contexts needs to be merged</param>
        public void Merge(EntityChangeContextCollection deltaEntityChangeContexts)
        {
            //Merge only if we have anything from delta.
            if (deltaEntityChangeContexts != null && deltaEntityChangeContexts.Count > 0)
            {
                //We don't have any into current collection. Take all data from delta and update to current collection.
                if (this == null || this.Count < 1)
                {
                    this._items = deltaEntityChangeContexts._items;
                }
                else
                {
                    foreach (EntityChangeContext deltaEntityChangeContext in deltaEntityChangeContexts)
                    {
                        Int64 entityId = deltaEntityChangeContext.EntityId;
                        ObjectAction currentAction = deltaEntityChangeContext.Action;

                        EntityChangeContext originalEntityChangeContext = this.Get(entityId);

                        //No previous entity change context found over here.. Add directly to current collection
                        if (originalEntityChangeContext == null)
                        {
                            this._items.Add(deltaEntityChangeContext);
                        }
                        else
                        {
                            ObjectAction originalAction = originalEntityChangeContext.Action;
                            ObjectAction mergedAction = EntityFamilyChangeContext.GetMergedAction(originalAction, currentAction);

                            originalEntityChangeContext.Action = mergedAction;
                            originalEntityChangeContext.LocaleChangeContexts.Merge(deltaEntityChangeContext.LocaleChangeContexts);
                        }
                    }
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// Gets entity change contexts for given entity type id
        /// </summary>
        /// <param name="entityTypeId">Indicates entity type for which entiy change context to be fetched</param>
        /// <returns>Returns entity change contexts based on given entity type id</returns>
        public EntityChangeContextCollection GetByEntityTypeId(Int32 entityTypeId)
        {
            EntityChangeContextCollection filteredEntityChangeContexts = null;

            if (this._items.Count > 0)
            {
                filteredEntityChangeContexts = new EntityChangeContextCollection();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    if (entityChangeContext.EntityTypeId == entityTypeId)
                    {
                        filteredEntityChangeContexts.Add(entityChangeContext);
                    }
                }
            }

            return filteredEntityChangeContexts;
        }

        /// <summary>
        /// Gets entity change contexts for given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entiy change context to be fetched</param>
        /// <returns>Returns entity change contexts based on given entity id</returns>
        public EntityChangeContext GetByEntityId(Int64 entityId)
        {
            if (this._items.Count > 0)
            {
                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    if (entityChangeContext.EntityId == entityId)
                    {
                        return entityChangeContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the attribute id list from entity change contexts
        /// </summary>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList()
        {
            Collection<Int32> attributeIdList = null;

            if (this._items.Count > 0)
            {
                attributeIdList = new Collection<Int32>();
                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    attributeIdList.AddRange<Int32>(entityChangeContext.LocaleChangeContexts.GetAttributeIdList());
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute ids from entity change contexts based on given object action
        /// </summary>
        /// <param name="action">Indicates object action</param>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList(ObjectAction action)
        {
            Collection<Int32> attributeIdList = null;

            if (this._items.Count > 0)
            {
                attributeIdList = new Collection<Int32>();
                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    attributeIdList.AddRange<Int32>(entityChangeContext.LocaleChangeContexts.GetAttributeIdList(action));
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute name list from entity change contexts
        /// </summary>
        /// <returns>Returns attribute name list</returns>
        public Collection<String> GetAttributeNameList()
        {
            Collection<String> attributeNameList = null;

            if (this._items.Count > 0)
            {
                attributeNameList = new Collection<String>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    attributeNameList.AddRange<String>(entityChangeContext.LocaleChangeContexts.GetAttributeNameList());
                }
            }

            return attributeNameList;
        }

        /// <summary>
        /// Gets the attribute locale list from entity change contexts
        /// </summary>
        /// <returns>Returns attribute locale list</returns>
        public Collection<LocaleEnum> GetAttributeLocaleList()
        {
            Collection<LocaleEnum> attributeLocaleList = null;

            if (this._items.Count > 0)
            {
                attributeLocaleList = new Collection<LocaleEnum>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    attributeLocaleList.AddRange<LocaleEnum>(entityChangeContext.LocaleChangeContexts.GetAttributeLocaleList());
                }
            }

            return attributeLocaleList;
        }

        /// <summary>
        /// Gets the relationship type id list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship type id list</returns>
        public Collection<Int32> GetRelationshipTypeIdList()
        {
            Collection<Int32> relationshipTypeIdList = null;

            if (this._items.Count > 0)
            {
                relationshipTypeIdList = new Collection<Int32>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    relationshipTypeIdList.AddRange<Int32>(entityChangeContext.LocaleChangeContexts.GetRelationshipTypeIdList());
                }
            }

            return relationshipTypeIdList;
        }

        /// <summary>
        /// Gets the relationship type name list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship type name list</returns>
        public Collection<String> GetRelationshipTypeNameList()
        {
            Collection<String> relationshipTypeNameList = null;

            if (this._items.Count > 0)
            {
                relationshipTypeNameList = new Collection<String>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    relationshipTypeNameList.AddRange<String>(entityChangeContext.LocaleChangeContexts.GetRelationshipTypeNameList());
                }
            }

            return relationshipTypeNameList;
        }

        /// <summary>
        /// Gets the relationship attribute id list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute id list</returns>
        public Collection<Int32> GetRelationshipAttributeIdList()
        {
            Collection<Int32> relationshipAttributeIdList = null;

            if (this._items.Count > 0)
            {
                relationshipAttributeIdList = new Collection<Int32>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    relationshipAttributeIdList.AddRange<Int32>(entityChangeContext.LocaleChangeContexts.GetRelationshipAttributeIdList());
                }
            }

            return relationshipAttributeIdList;
        }

        /// <summary>
        /// Gets the relationship attribute name list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute name list</returns>
        public Collection<String> GetRelationshipAttributeNameList()
        {
            Collection<String> relationshipAttributeNameList = null;

            if (this._items.Count > 0)
            {
                relationshipAttributeNameList = new Collection<String>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    relationshipAttributeNameList.AddRange<String>(entityChangeContext.LocaleChangeContexts.GetRelationshipAttributeNameList());
                }
            }

            return relationshipAttributeNameList;
        }

        /// <summary>
        /// Gets the relationship attribute locale list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute locale list</returns>
        public Collection<LocaleEnum> GetRelationshipAttributeLocaleList()
        {
            Collection<LocaleEnum> relationshipAttributeLocaleList = null;

            if (this._items.Count > 0)
            {
                relationshipAttributeLocaleList = new Collection<LocaleEnum>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    relationshipAttributeLocaleList.AddRange<LocaleEnum>(entityChangeContext.LocaleChangeContexts.GetRelationshipAttributeLocaleList());
                }
            }

            return relationshipAttributeLocaleList;
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasRelationshipsChanged()
        {
            Boolean hasRelationshipsChanged = false;

            if (this._items.Count > 0)
            {
                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    hasRelationshipsChanged = entityChangeContext.LocaleChangeContexts.HasRelationshipsChanged();

                    if (hasRelationshipsChanged)
                    {
                        break;
                    }
                }
            }

            return hasRelationshipsChanged;
        }

        /// <summary>
        /// Checks whether any entity change context action is Create 
        /// </summary>
        /// <returns>Return true if object is having Create action</returns>
        public Boolean HasEntitiesCreated()
        {
            Boolean hasEntitiesCreated = false;

            if (this._items.Count > 0)
            {
                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    if (entityChangeContext.Action == ObjectAction.Create)
                    {
                        hasEntitiesCreated = true;
                        break;
                    }
                }
            }

            return hasEntitiesCreated;
        }

        /// <summary>
        /// Gets entity id list based on given object action.
        /// </summary>
        /// <param name="objectActions">Indicates collection of object action</param>
        /// <returns>Returns collection of entity id list for given object action</returns>
        public Collection<Int64> GetEntityIdList(Collection<ObjectAction> objectActions)
        {
            Collection<Int64> entityIdList = null;

            if (this._items.Count > 0)
            {
                entityIdList = new Collection<Int64>();

                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    if (objectActions.Contains(entityChangeContext.Action))
                    {
                        entityIdList.Add(entityChangeContext.EntityId);
                    }
                }
            }

            return entityIdList;
        }

        #endregion Helper Methods

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityChangeContextCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityChangeContext")
                        {
                            String entityChangeContextsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(entityChangeContextsXml))
                            {
                                EntityChangeContext entityChangeContext = new EntityChangeContext(entityChangeContextsXml);

                                if (entityChangeContext != null)
                                {
                                    this.Add(entityChangeContext);
                                }
                            }
                        }
                        else
                        {
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
        }

        /// <summary>
        /// Gets entity change context for a given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <returns>Returns entity change context based on given entity id</returns>
        private EntityChangeContext Get(Int64 entityId)
        {
            if (this._items.Count > 0)
            {
                foreach (EntityChangeContext entityChangeContext in this._items)
                {
                    if (entityChangeContext.EntityId == entityId)
                    {
                        return entityChangeContext;
                    }
                }
            }

            return null;
        }

        #endregion Private Methods

        #endregion Methods
    }
}