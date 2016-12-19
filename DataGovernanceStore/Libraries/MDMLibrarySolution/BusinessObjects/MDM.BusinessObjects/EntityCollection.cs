using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Entity Instance Collection for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityCollection : ICollection<Entity>, IEnumerable<Entity>, IEntityCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        [ProtoMember(1)]
        private Collection<Entity> _entities = new Collection<Entity>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityCollection(String valuesAsXml)
        {
            LoadEntityCollection(valuesAsXml, ObjectSerialization.Full);
        }

        /// <summary>
        /// Constructor which takes Xml as an input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public EntityCollection(String valuesAsXml, ObjectSerialization objectSerialization)
        {
            LoadEntityCollection(valuesAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize EntityCollection from IList of value
        /// </summary>
        /// <param name="entityList">List of Entity object</param>
        public EntityCollection(IList<Entity> entityList)
        {
            this._entities = new Collection<Entity>(entityList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find entity from EntityCollection based on entityId
        /// </summary>
        /// <param name="entityId">EntityID to search</param>
        /// <returns>Entity object having given entityID</returns>
        public Entity this[Int64 entityId]
        {
            get
            {
                Entity entity = (Entity)GetEntity(entityId);
                if (entity == null)
                    throw new ArgumentException(String.Format("No entity found for entity id: {0}", entityId), "entityId");
                else
                    return entity;
            }
            set
            {
                Entity entity = (Entity)GetEntity(entityId);

                if (entity == null)
                    throw new ArgumentException(String.Format("No entity found for entity id: {0}", entityId), "entityId");

                entity = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Check if EntityCollection contains entity with given entityId
        /// </summary>
        /// <param name="entityId">EntityId to search in EntityCollection</param>
        /// <returns>
        /// <para>true : If entity found in entityCollection</para>
        /// <para>false : If entity found not in entityCollection</para>
        /// </returns>
        public bool Contains(Int64 entityId)
        {
            if (GetEntity(entityId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entity object from EntityCollection
        /// </summary>
        /// <param name="entityId">Id of entity which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int64 entityId)
        {
            Entity entity = (Entity)GetEntity(entityId);

            if (entity == null)
                throw new ArgumentException("No entity found for given entity id");
            else
                return this.Remove(entity);
        }

        /// <summary>
        /// Remove entity object from EntityCollection
        /// </summary>
        /// <param name="externalId">External id of entity which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(String externalId)
        {
            Entity entity = GetEntityByExternalId(externalId);

            if (entity == null)
                throw new ArgumentException("No entity found for given external id");
            else
                return this.Remove(entity);
        }

        /// <summary>
        /// Remove entity object from EntityCollection
        /// </summary>
        /// <param name="referenceId">Reference id of entity which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool RemoveByReferenceId(Int64 referenceId)
        {
            Entity entity = GetEntityByReferenceId(referenceId);

            if (entity == null)
                throw new ArgumentException("No entity found for given reference id");
            else
                return this.Remove(entity);
        }

        /// <summary>
        /// Get the Entity Ids in a delimited string format
        /// </summary>
        /// <returns>Entity Id list </returns>
        public new String ToString()
        {
            if (_entities.Count < 1)
                return String.Empty;

            String entityString = String.Empty;
            foreach (Entity entity in _entities)
            {
                entityString = entityString + entity.Id + ",";
            }
            return entityString.Substring(0, entityString.Length - 1);
        }

        /// <summary>
        /// Get the Entity Names in a delimited string format
        /// </summary>
        /// <returns>Entity Id list </returns>
        public String GetEntityNamesString()
        {
            if (_entities.Count < 1)
                return String.Empty;

            String entityString = String.Empty;

            foreach (Entity entity in _entities)
            {
                entityString = entityString + entity.Name + ",";
            }

            return entityString.Substring(0, entityString.Length - 1);
        }

        /// <summary>
        /// Get Xml representation of EntityCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityCollection</returns>
        public String ToXml()
        {
            String entitysXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Entity entity in this._entities)
            {
                builder.Append(entity.ToXml());
            }

            entitysXml = String.Format("<Entities>{0}</Entities>", builder.ToString());
            return entitysXml;
        }

        /// <summary>
        /// Get Xml representation of EntityCollection object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
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
            String entitysXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Entity entity in this._entities)
            {
                builder.Append(entity.ToXml(objectSerialization, serializeOnlyRootElements));
            }

            entitysXml = String.Format("<Entities>{0}</Entities>", builder.ToString());
            return entitysXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is EntityCollection)
            {
                EntityCollection objectToBeCompared = obj as EntityCollection;
                Int32 entitysUnion = this._entities.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entitysIntersect = this._entities.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (entitysUnion != entitysIntersect)
                    return false;
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
            foreach (Entity attr in this._entities)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        ///  The initial load process needs to get the entity using the external id.
        /// </summary>
        /// <param name="externalId">Indicates externalId of an entity which is to be loaded</param>
        /// <returns>Returns filtered entities</returns>
        public Entity GetEntityByExternalId(String externalId)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                return null;
            }

            var filteredEntities = from entity in this._entities
                                   where
                                   (entity.ExternalId == externalId)
                                   select entity;

            if (filteredEntities.Any())
                return filteredEntities.First();
            else
                return null;
        }

        /// <summary>
        ///  It returns the entity using the short name.
        /// </summary>
        /// <param name="shortName">Indicates shortName of an entity which is to be loaded</param>
        /// <returns>Returns filtered entities</returns>
        public Entity GetEntityByName(String shortName)
        {
            if (string.IsNullOrEmpty(shortName))
            {
                return null;
            }

            var filteredEntities = from entity in this._entities
                                   where
                                   (entity.Name == shortName)
                                   select entity;

            if (filteredEntities.Any())
                return filteredEntities.First();
            else
                return null;
        }

        /// <summary>
        /// Gets the entity using the short name and category path.
        /// </summary>
        /// <param name="entityName">Indicates shortName of an entity which is to be loaded</param>
        /// <param name="categoryPath">The category path.</param>
        /// <returns>
        /// Returns filtered entity
        /// </returns>
        public Entity GetEntityByNameAndCategoryPath(String entityName, String categoryPath)
        {
            if (String.IsNullOrWhiteSpace(entityName) || String.IsNullOrWhiteSpace(categoryPath))
            {
                return null;
            }

            foreach (Entity entity in _entities)
            {
                if (String.Compare(entity.Name, entityName) == 0 && String.Compare(entity.CategoryPath, categoryPath) == 0)
                {
                    return entity;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the entity using the short name and container name
        /// </summary>
        /// <param name="entityName">Indicates shortName of an entity which is to be loaded</param>
        /// <param name="containerName">Indicates container name entity is to be loaded from</param>
        /// <returns>
        /// Returns filtered entity
        /// </returns>
        public Entity GetEntityByNameAndContainerName(String entityName, String containerName)
        {
            if (String.IsNullOrWhiteSpace(entityName) || String.IsNullOrWhiteSpace(containerName))
            {
                return null;
            }

            foreach (Entity entity in _entities)
            {
                if (String.Compare(entity.Name, entityName) == 0 && String.Compare(entity.ContainerName, containerName) == 0)
                {
                    return entity;
                }
            }

            return null;
        }

        /// <summary>
        ///  The initial load process needs to get the entity using the refernece id.
        /// </summary>
        /// <param name="referenceId">Indicates referenceId of an entity which is to be loaded</param>
        /// <returns>Returns filtered entities</returns>
        public Entity GetEntityByReferenceId(Int64 referenceId)
        {
            var filteredEntities = from entity in this._entities
                                   where
                                   (entity.ReferenceId == referenceId)
                                   select entity;

            if (filteredEntities.Any())
                return filteredEntities.First();
            else
                return null;
        }

        /// <summary>
        /// Get list of entities based on the specified external id.
        /// The initial load process needs to get the entities using the external id.entitiesLoadedFromDB
        /// </summary>
        /// <param name="externalId">Indicates the external id based on which the list of entities to be fetched</param>
        /// <returns>Returns a list of entities based on the specified external id</returns>
        public List<Entity> GetEntitiesByExternalId(String externalId)
        {
            if (string.IsNullOrWhiteSpace(externalId))
            {
                return null;
            }

            var filteredEntities = new List<Entity>();

            foreach (var entity in _entities)
            {
                if (entity.ExternalId.Equals(externalId))
                {
                    filteredEntities.Add(entity);
                }
            }

            return filteredEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentEntityId"></param>
        /// <returns></returns>
        public EntityCollection GetEntitiesByParentEntityId(Int64 parentEntityId)
        {
            var childEntities = new EntityCollection();

            foreach (var entity in _entities)
            {
                if (entity.ParentEntityId.Equals(parentEntityId))
                {
                    childEntities.Add(entity);
                }
            }

            return childEntities;
        }

        /// <summary>
        /// Gets entities based on given parent extension entity id
        /// </summary>
        /// <param name="parentExtensionEntityId">Indicates parent extension entity id</param>
        /// <returns>Returns entities for a given parent extension entity id</returns>
        public EntityCollection GetEntitiesByParentExtensionEntityId(Int64 parentExtensionEntityId)
        {
            var childEntities = new EntityCollection();

            foreach (var entity in _entities)
            {
                if (entity.ParentExtensionEntityId == parentExtensionEntityId)
                {
                    childEntities.Add(entity);
                }
            }

            return childEntities;
        }

        /// <summary>
        /// Get list of entities based on the specified entity type id.
        /// </summary>
        /// <param name="entityTypeId">Indicates entity type id</param>
        /// <returns></returns>
        public IEntityCollection GetEntitiesByEntityTypeId(Int32 entityTypeId)
        {
            var filteredEntities = new EntityCollection();

            foreach (var entity in _entities)
            {
                if (entity.EntityTypeId.Equals(entityTypeId))
                {
                    filteredEntities.Add(entity);
                }
            }

            return filteredEntities;
        }

        /// <summary>
        /// Get list of entities based on the specified container id.
        /// </summary>
        /// <param name="containerId">Indicates container id</param>
        /// <returns></returns>
        public IEntityCollection GetEntitiesByContainerId(Int32 containerId)
        {
            var filteredEntities = new EntityCollection();

            foreach (var entity in _entities)
            {
                if (entity.ContainerId.Equals(containerId))
                {
                    filteredEntities.Add(entity);
                }
            }

            return filteredEntities;
        }

        /// <summary>
        ///  The initial load process needs to get the relationship using the external id.
        /// </summary>
        /// <param name="externalId">Indicates externalId of an relationship which is to be loaded</param>
        /// <returns>Returns filtered relationship</returns>
        public Relationship GetRelationshipByExternalId(String externalId)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                return null;
            }

            foreach (Entity entity in this._entities)
            {
                foreach (Relationship relationship in entity.Relationships)
                {
                    if (relationship.RelationshipExternalId == externalId)
                    {
                        return relationship;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets entity id list from the current entity collection
        /// </summary>
        /// <returns>Collection of Int64 of entity ids in current collection</returns>
        /// <exception cref="Exception">Thrown if there are no entities in current entity collection</exception>
        public Collection<Int64> GetEntityIdList()
        {
            #region Parameter validation

            if (this._entities == null) //|| this._entities.Count < 1)
            {
                throw new Exception("There are no entities in collection. Cannot get EntityIds");
            }

            #endregion Parameter validation

            var entityIdList = new Collection<Int64>();

            foreach (var entity in _entities)
            {
                if (!entityIdList.Contains(entity.Id))
                {
                    entityIdList.Add(entity.Id);
                }
            }

            return entityIdList;
        }

        /// <summary>
        /// Get splitted entity id list based on entity types from the current entity collection
        /// </summary>
        /// <returns>Collection of Int64 of entity ids in current collection</returns>
        /// <exception cref="Exception">Thrown if there are no entities in current entity collection</exception>
        public Dictionary<Int32, Collection<Int64>> GetAndSplitEntityIdListByEntityTypes()
        {
            #region Parameter validation

            if (this._entities == null) //|| this._entities.Count < 1)
            {
                throw new Exception("There are no entities in collection. Cannot get EntityIds");
            }

            #endregion Parameter validation

            Dictionary<Int32, Collection<Int64>> entityIdListBsedonEntityType = new Dictionary<Int32, Collection<Int64>>();
            Collection<Int64> entityIdList = null;

            foreach (var entity in _entities)
            {
                if (entityIdListBsedonEntityType.TryGetValue(entity.EntityTypeId, out entityIdList))
                {
                    if (!entityIdList.Contains(entity.Id))
                    {
                        entityIdList.Add(entity.Id);
                    }
                }
                else
                {
                    entityIdList = new Collection<Int64>() { entity.Id };
                    entityIdListBsedonEntityType.Add(entity.EntityTypeId, entityIdList);
                }
            }

            return entityIdListBsedonEntityType;
        }

        /// <summary>
        /// Gets entity type id list from the current entity collection
        /// </summary>
        /// <returns>Collection of Int32 of entity type ids in current collection</returns>
        public Collection<Int32> GetEntityTypeIdList()
        {
            var entityTypeIdList = new Collection<Int32>();
            
            if (this._entities == null || this._entities.Count < 1)
            {
                return entityTypeIdList;
            }
            
            foreach (var entity in _entities)
            {
                if (!entityTypeIdList.Contains(entity.EntityTypeId))
                {
                    entityTypeIdList.Add(entity.EntityTypeId);
                }
            }

            return entityTypeIdList;
        }

        /// <summary>
        /// Gets container id list from the current entity collection
        /// </summary>
        /// <returns>Collection of Int32 of container ids in current collection</returns>
        public Collection<Int32> GetContainerIdList()
        {
            var containerIdList = new Collection<Int32>();

            if (this._entities == null || this._entities.Count < 1)
            {
                return containerIdList;
            }

            foreach (var entity in _entities)
            {
                if (!containerIdList.Contains(entity.ContainerId))
                {
                    containerIdList.Add(entity.ContainerId);
                }
            }

            return containerIdList;
        }

        /// <summary>
        /// Gets category id list from the current entity collection
        /// </summary>
        /// <returns>Collection of Int32 of category ids in current collection</returns>
        public Collection<Int64> GetCategoryIdList()
        {
            var categoryIdList = new Collection<Int64>();

            if (this._entities == null || this._entities.Count < 1)
            {
                return categoryIdList;
            }

            foreach (var entity in _entities)
            {
                if (!categoryIdList.Contains(entity.CategoryId))
                {
                    categoryIdList.Add(entity.CategoryId);
                }
            }

            return categoryIdList;
        }

        /// <summary>
        /// Compare entityCollection with current collection.
        /// This method will compare entity. If current collection has more entities than object to be compared, extra entities will be ignored.
        /// If entity to be compared has entities which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetEntityCollection">EntityCollection to be compared with current collection</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public bool IsSuperSetOf(EntityCollection subsetEntityCollection)
        {
            if (subsetEntityCollection != null)
            {
                foreach (Entity entity in subsetEntityCollection)
                {
                    //Get sub set entity from super entity collection.
                    IEntity iEntity = this.GetEntityByName(entity.Name);

                    //If it doesn't return, that means super set doesn't contain that entity.
                    //So return false, else do further comparison
                    if (iEntity != null)
                    {
                        Entity sourceEntity = (Entity)iEntity;

                        if (!sourceEntity.IsSuperSetOf(entity))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Compare 2 entity collections, return EntityOperationResultCollection with error identifying mismatch(es)
        /// Operation Result is sucessful if no mismatch found.
        /// </summary>
        /// <param name="subsetEntityCollection"></param>
        /// <returns></returns>
        public EntityOperationResultCollection GetSuperSetOperationResultCollection(EntityCollection subsetEntityCollection)
        {
            var entityOperationResultCollection = new EntityOperationResultCollection();

            entityOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.Successful;

            if (subsetEntityCollection != null)
            {
                foreach (Entity entity in subsetEntityCollection)
                {
                    //Get sub set entity from super entity collection.
                    IEntity iEntity = this.GetEntityByName(entity.Name);

                    //If it doesn't return, that means super set doesn't contain that entity.
                    //So return false, else do further comparison
                    if (iEntity != null)
                    {
                        var sourceEntity = (Entity)iEntity;
                        var entityOperationResult = sourceEntity.GetSuperSetOperationResult(entity);
                        entityOperationResultCollection.Add(entityOperationResult);
                    }
                    else
                    {
                        entityOperationResultCollection.AddOperationResult("-1", String.Format(" Entity {0} is null", entity.Name), OperationResultType.Error);
                    }
                }
            }
            else
            {
                entityOperationResultCollection.AddOperationResult("-1", "subsetEntityCollection is null", OperationResultType.Error);
            }

            entityOperationResultCollection.RefreshOperationResultStatus();
            return entityOperationResultCollection;
        }

        /// <summary>
        /// Gets the Entity from the collection based on External Id
        /// </summary>
        /// <param name="entityExternalId">External Id of entity to search in current collection</param>
        /// <returns>Entity having given Id</returns>
        public IEntity GetEntity(String entityExternalId)
        {
            IEntity entityToReturn = null;

            if (!String.IsNullOrWhiteSpace(entityExternalId))
            {
                if (this._entities != null)
                {
                    foreach (Entity currentEntity in this._entities)
                    {
                        if (String.Compare(currentEntity.ExternalId, entityExternalId, true) == 0)
                        {
                            entityToReturn = currentEntity;
                            break;
                        }
                    }
                }
            }

            return entityToReturn;
        }

        /// <summary>
        /// Get list of entities based on the specified entity type id.
        /// </summary>
        /// <param name="entityFamilyId">Entity family id of entity to search in current collection</param>
        /// <returns>Returns collection of entity having given family id</returns>
        public IEntityCollection GetEntitiesByFamilyId(Int64 entityFamilyId)
        {
            return FindEntities(entity => entity.EntityFamilyId == entityFamilyId);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load current EntityCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current EntityCollection</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadEntityCollection(String valuesAsXml, ObjectSerialization objectSerialization)
        {
            #region Sample Xml
            /*
            * <Entities>
                <Entity Id="4376254" ObjectId="318823" Name="912904" LongName="912904" SKU="912904" Path="Bluestem Brands#@#VP Product Staging Catalog#@#CS#@#066#@#430#@#912904" BranchLevel="2" ParentEntityId="402" ParentEntityName="" ParentEntityLongName="Cosmetics / Fragrance + Cosmetics / Fragrance + Accessories" CategoryId="0" CategoryName="" CategoryLongName="" ContainerId="1206" ContainerName="" ContainerLongName="VP Product Staging Catalog" OrganizationId="4" OrganizationName="" OrganizationLongName="" EntityTypeId="7" EntityTypeName="Product" EntityTypeLongName="" Locale="en_WW" Action="Read" EffectiveFrom="" EffectiveTo="" EntityLastModTime="" EntityLastModUser="">
                  <Attributes>
                    <Attribute Id="3003" Name="" LongName="" InstanceRefId="-1" AttributeParentId="0" AttributeParentName="" AttributeParentLongName="" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttributeType="Simple" AttributeModelType="All" AttributeDataType="" SourceFlag="O" Action="Read">
                      <Values SourceFlag="O">
                        <Value Uom="" ValueRefId="0" Sequence="-1" Locale="en_WW"><![CDATA[]]></Value>
                      </Values>
                      <Values SourceFlag="I" />
                      <Attributes />
                    </Attribute>
                  </Attributes>
                </Entity>
                <Entity Id="4376255" ObjectId="318824" Name="116674" LongName="116674" SKU="116674" Path="Bluestem Brands#@#VP Product Staging Catalog#@#AP#@#116674" BranchLevel="2" ParentEntityId="10" ParentEntityName="" ParentEntityLongName="Apparel" CategoryId="0" CategoryName="" CategoryLongName="" ContainerId="1206" ContainerName="" ContainerLongName="VP Product Staging Catalog" OrganizationId="4" OrganizationName="" OrganizationLongName="" EntityTypeId="7" EntityTypeName="Product" EntityTypeLongName="" Locale="en_WW" Action="Read" EffectiveFrom="" EffectiveTo="" EntityLastModTime="" EntityLastModUser="">
                  <Attributes>
                    <Attribute Id="3003" Name="" LongName="" InstanceRefId="-1" AttributeParentId="0" AttributeParentName="" AttributeParentLongName="" IsCollection="False" IsComplex="False" IsLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" Precision="0" AttributeType="Simple" AttributeModelType="All" AttributeDataType="" SourceFlag="O" Action="Read">
                      <Values SourceFlag="O">
                        <Value Uom="" ValueRefId="0" Sequence="-1" Locale="en_WW"><![CDATA[]]></Value>
                      </Values>
                      <Values SourceFlag="I" />
                      <Attributes />
                    </Attribute>
                  </Attributes>
                </Entity>
              </Entities>
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
                            String entityXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityXml))
                            {
                                Entity entity = new Entity(entityXml, objectSerialization);
                                if (entity != null)
                                {
                                    this.Add(entity);
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

        #endregion Private Methods

        #region ICollection<Entity> Members

        /// <summary>
        /// Add entity object in collection
        /// </summary>
        /// <param name="item">entity to add in collection</param>
        public void Add(Entity item)
        {
            this._entities.Add(item);
        }

        /// <summary>
        /// Add a list of entity objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of entity objects</param>
        public void Add(List<Entity> items)
        {
            foreach (Entity item in items)
            {
                // check if the item is already available..if not add..
                if (!Contains(item))
                    this.Add(item);
            }
        }

        /// <summary>
        /// Add entity in collection
        /// </summary>
        /// <param name="iEntity">entity to add in collection</param>
        public void Add(IEntity iEntity)
        {
            if (iEntity != null)
            {
                this.Add((Entity)iEntity);
            }
        }

        /// <summary>
        /// Add Entities in collection
        /// </summary>
        /// <param name="iEntityCollection">Indicates entities to add in collection</param>
        /// <param name="checkDuplicates">Indicates whether duplicate entities can be added in current collection of entities</param>
        public void AddRange(IEntityCollection iEntityCollection, Boolean checkDuplicates = false)
        {
            if (iEntityCollection == null)
            {
                throw new ArgumentNullException("EntityCollection");
            }

            foreach (Entity entity in iEntityCollection)
            {
                Boolean isDuplicate = false;
                if (checkDuplicates)
                {
                    foreach (Entity item in this)
                    {
                        if (item.Id == entity.Id)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }

                    if (!isDuplicate)
                    {
                        this.Add(entity);
                    }
                }
                else
                {
                    this.Add(entity);
                }
            }
        }

        /// <summary>
        /// Add Entities in collection
        /// </summary>
        /// <param name="entities">Entities to add in collection</param>
        public void AddRange(EntityCollection entities)
        {
            this.AddRange((IEntityCollection)entities);
        }

        /// <summary>
        /// Removes all entities from collection
        /// </summary>
        public void Clear()
        {
            this._entities.Clear();
        }

        /// <summary>
        /// Determines whether the EntityCollection contains a specific entity.
        /// </summary>
        /// <param name="item">The entity object to locate in the EntityCollection.</param>
        /// <returns>
        /// <para>true : If entity found in entityCollection</para>
        /// <para>false : If entity found not in entityCollection</para>
        /// </returns>
        public bool Contains(Entity item)
        {
            return this._entities.Contains(item);
        }

        /// <summary>
        ///  Copies the elements of the EntityCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Entity[] array, int arrayIndex)
        {
            this._entities.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entities in EntityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entities.Count;
            }
        }

        /// <summary>
        /// Check if EntityCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityCollection.
        /// </summary>
        /// <param name="item">The entity object to remove from the EntityCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityCollection</returns>
        public bool Remove(Entity item)
        {
            return this._entities.Remove(item);
        }

        #endregion

        #region IEnumerable<Entity> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Entity> GetEnumerator()
        {
            return this._entities.GetEnumerator();
        }

        #endregion IEnumerable<Entity> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entities.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IEntityCollection Members

        /// <summary>
        /// Get Entity from the collection based on EntityId
        /// </summary>
        /// <param name="entityId">Id of entity to search in current collection</param>
        /// <returns>Entity having given Id</returns>
        /// <exception cref="ArgumentException">Entity Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no entities to search in</exception>
        public IEntity GetEntity(Int64 entityId)
        {
            if (this._entities == null)
            {
                throw new NullReferenceException("There are no entities to search in");
            }

            Entity entityToReturn = null;

            foreach (Entity entity in this._entities)
            {
                if (entity.Id.Equals(entityId))
                {
                    entityToReturn = entity;
                    break;
                }
            }

            return entityToReturn;
        }

        /// <summary>
        /// Gets entities based on compare method
        /// </summary>
        /// <param name="compareMethod">Indicates a condition based on what entity can be processed further.</param>
        private IEntityCollection FindEntities(Func<IEntity, Boolean> compareMethod)
        {
            EntityCollection filteredEntities = null;

            if (this._entities != null && this._entities.Count > 0)
            {
                filteredEntities = new EntityCollection();

                foreach (var entity in this._entities)
                {
                    if (compareMethod.Invoke(entity))
                    {
                        filteredEntities.Add(entity);
                    }
                }
            }

            return filteredEntities;
        }

        #endregion IEntityCollection Members

        #endregion Methods
    }
}