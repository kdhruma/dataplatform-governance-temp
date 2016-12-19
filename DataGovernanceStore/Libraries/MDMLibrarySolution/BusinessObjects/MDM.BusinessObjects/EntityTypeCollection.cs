using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies Entity Type Collection
    /// </summary>
    [DataContract]
    public class EntityTypeCollection : ICollection<EntityType>, IEnumerable<EntityType>, IEntityTypeCollection , IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of entity Type
        /// </summary>
        [DataMember]
        private Collection<EntityType> _entityTypes = new Collection<EntityType>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Entity Type Collection
        /// </summary>
        public EntityTypeCollection() : base() { }

        /// <summary>
        /// Initialize EntityTypeCollection from IList of value
        /// </summary>
        /// <param name="entityTypeList">List of EntityType object</param>
        public EntityTypeCollection(IList<EntityType> entityTypeList)
        {
            this._entityTypes = new Collection<EntityType>(entityTypeList);
        }

        /// <summary>
		/// Constructor which takes Xml as input
		/// </summary>
        public EntityTypeCollection(String valueAsXml)
		{
			LoadEntityTypeCollection(valueAsXml);
		}

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return ObjectType.EntityType;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<EntityType> GetInternalCollection()
        {
            return _entityTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        public EntityType Get(Int32 entityTypeId)
        {
            return GetEntityType(entityTypeId);
        }

        /// <summary>
        /// Gets entity types based on given entity type id list
        /// </summary>
        /// <param name="entityTypeIdList">Indicates entity type id list to be retrieved from collection</param>
        /// <returns>collection of entity types</returns>
        public EntityTypeCollection Get(Collection<Int32> entityTypeIdList)
        {
            return GetEntityTypes(entityTypeIdList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        public EntityType Get(String entityTypeName)
        {
            return GetEntityType(entityTypeName);
        }

        /// <summary>
                /// Clone EntityType collection.
                /// </summary>
                /// <returns>cloned EntityType collection object.</returns>
        public IEntityTypeCollection Clone()
        {
            EntityTypeCollection clonedEntityTypes = new EntityTypeCollection();

            if (this._entityTypes != null && this._entityTypes.Count > 0)
            {
                foreach (EntityType entityType in this._entityTypes)
                {
                    IEntityType clonedIEntityType = entityType.Clone();
                    clonedEntityTypes.Add(clonedIEntityType);
                }
            }

            return clonedEntityTypes;
        }

        /// <summary>
        /// Check if EntityTypeCollection contains entity Type with given typeId
        /// </summary>
        /// <param name="typeId">typeId using which entity Type is to be searched from collection</param>
        /// <returns>
        /// <para>true : If entity type found in EntityTypeCollection</para>
        /// <para>false : If entity type found not in EntityTypeCollection</para>
        /// </returns>
        public bool Contains(Int32 typeId)
        {
            if (GetEntityType(typeId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entity type object from EntityTypeCollection
        /// </summary>
        /// <param name="typeId">typeId of entity type which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 typeId)
        {
            EntityType entityType = GetEntityType(typeId);

            if (entityType == null)
                throw new ArgumentException("No entity type found for given typeId");
            else
                return this.Remove(entityType);
        }

        /// <summary>
        /// Get Xml representation of Entity Type Collection
        /// </summary>
        /// <returns>Xml representation of Entity Type Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntityTypes>";

            if (this._entityTypes != null && this._entityTypes.Count > 0)
            {
                foreach (EntityType type in this._entityTypes)
                {
                    returnXml = String.Concat(returnXml, type.ToXML());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityTypes>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityTypeCollection)
            {
                EntityTypeCollection objectToBeCompared = obj as EntityTypeCollection;

                Int32 entityTypesUnion = this._entityTypes.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityTypesIntersect = this._entityTypes.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityTypesUnion != entityTypesIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (EntityType entityType in this._entityTypes)
            {
                hashCode += entityType.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region ICollection<EntityType> Members

        /// <summary>
        /// Add entity type object in collection
        /// </summary>
        /// <param name="type">entity type to add in collection</param>
        public void Add(EntityType type)
        {
            this._entityTypes.Add(type);
        }

        /// <summary>
        /// Add entity type object in collection
        /// </summary>
        /// <param name="type">entity type to add in collection</param>
        public void Add(IEntityType type)
        {
            if (type != null)
            {
                Add((EntityType)type);
            }
        }

        /// <summary>
        /// Adds passed entityTypes to the current collection
        /// </summary>
        /// <param name="entityTypes">Collection of entityTypes which needs to be added</param>
        public void AddRange(EntityTypeCollection entityTypes)
        {
            if (entityTypes != null && entityTypes.Count > 0)
            {
                foreach (EntityType entityType in entityTypes)
                {
                    this.Add(entityType);
                }
            }
        }

        /// <summary>
        /// Removes all entity type from collection
        /// </summary>
        public void Clear()
        {
            this._entityTypes.Clear();
        }

        /// <summary>
        /// Determines whether the EntityTypeCollection contains a specific entity type
        /// </summary>
        /// <param name="type">The entity type object to locate in the EntityTypeCollection.</param>
        /// <returns>
        /// <para>true : If entity type found in EntityTypeCollection</para>
        /// <para>false : If entity type found not in EntityTypeCollection</para>
        /// </returns>
        public bool Contains(EntityType type)
        {
            return this._entityTypes.Contains(type);
        }

        /// <summary>
        /// Copies the elements of the EntityTypeCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityType[] array, int arrayIndex)
        {
            this._entityTypes.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entity types in EntityTypeCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityTypes.Count;
            }
        }

        /// <summary>
        /// Check if EntityTypeCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity type from the EntityTypeCollection.
        /// </summary>
        /// <param name="type">The entity type object to remove from the EntityTypeCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntityType type)
        {
            return this._entityTypes.Remove(type);
        }

        #endregion

        #region IEnumerable<EntityType> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityTypeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityType> GetEnumerator()
        {
            return this._entityTypes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityTypeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityTypes.GetEnumerator();
        }

        #endregion

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> entityTypesInBatch = null;

            if (this._entityTypes != null)
            {
                entityTypesInBatch = Utility.Split(this, batchSize);
            }

            return entityTypesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as EntityType);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetEntityTypeCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>        
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityTypeCollection subsetEntityTypeCollection, Boolean compareIds = false)
        {
                if (subsetEntityTypeCollection != null && subsetEntityTypeCollection.Count > 0)
                {
                    foreach (EntityType entityType in subsetEntityTypeCollection)
                    {
                        //Get subset entity type from super entity type collection.
                        EntityType sourceEntityType = this.Get(entityType.Name);

                        if (sourceEntityType != null)
                        {
                            if (!sourceEntityType.IsSuperSetOf(entityType, compareIds))
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }
                }
            return true;
        }

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            EntityTypeCollection entityTypes = GetEntityTypes(referenceIds);

            if (entityTypes != null && entityTypes.Count > 0)
            {
                foreach (EntityType entityType in entityTypes)
                {
                    result = result && this.Remove(entityType);
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize EntityTypeCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">>Xml having value for EntityTypeCollection</param>
        private void LoadEntityTypeCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityType")
                        {
                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                EntityType entityType = new EntityType(attributeXml);
                                if (entityType != null)
                                {
                                    this.Add(entityType);
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
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        private EntityType GetEntityType(Int32 entityTypeId)
        {
            var filteredTypes = GetEntityTypes(new Collection<Int32>() { entityTypeId });

            if (filteredTypes != null && filteredTypes.Any())
                return filteredTypes.First();
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeIdList"></param>
        /// <returns></returns>
        private EntityTypeCollection GetEntityTypes(Collection<Int32> entityTypeIdList)
        {
            EntityTypeCollection entityTypes = null;

            if (entityTypeIdList != null && entityTypeIdList.Count > 0 && this._entityTypes.Count > 0)
            {
                entityTypes = new EntityTypeCollection();

                foreach (EntityType entityType in this._entityTypes)
                {
                    if (entityTypeIdList.Contains(entityType.Id))
                    {
                        entityTypes.Add(entityType);
                    }

                    if (entityTypes.Count.Equals(entityTypeIdList.Count))
                    {
                        break;
                    }
                }
            }

            return entityTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private EntityType GetEntityType(String typeName)
        {
            typeName = typeName.ToLowerInvariant();

            var filteredTypes = from type in this._entityTypes
                                where type.Name.ToLowerInvariant().Equals(typeName)
                                select type;

            if (filteredTypes.Any())
                return filteredTypes.First();
            else
                return null;
        }

        /// <summary>
        ///  Gets the entity type using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>Returns filtered entity types</returns>
        private EntityTypeCollection GetEntityTypes(Collection<String> referenceIds)
        {
            EntityTypeCollection entityTypes = new EntityTypeCollection();
            Int32 counter = 0;

            if (this._entityTypes.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (EntityType entityType in this._entityTypes)
                {
                    if (referenceIds.Contains(entityType.ReferenceId))
                    {
                        entityTypes.Add(entityType);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return entityTypes;
        }

        #endregion

        #endregion
    }
}