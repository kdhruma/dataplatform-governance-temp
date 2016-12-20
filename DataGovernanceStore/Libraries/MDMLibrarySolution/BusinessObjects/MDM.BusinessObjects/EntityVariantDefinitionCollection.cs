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
    /// Specifies Entity Variant Definition Collection
    /// </summary>
    [DataContract]
    public class EntityVariantDefinitionCollection : ICollection<EntityVariantDefinition>, IEnumerable<EntityVariantDefinition>, 
                                                        IEntityVariantDefinitionCollection, IDataModelObjectCollection, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of entity variant definition
        /// </summary>
        [DataMember]
        private Collection<EntityVariantDefinition> _entityVariantDefinitions = new Collection<EntityVariantDefinition>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the entity variant definition collection
        /// </summary>
        public EntityVariantDefinitionCollection() : base() { }

        /// <summary>
        /// Initialize EntityVariantDefinitionCollection from IList of value
        /// </summary>
        /// <param name="entityVariantDefinitionList">List of EntityVariantDefinition object</param>
        public EntityVariantDefinitionCollection(IList<EntityVariantDefinition> entityVariantDefinitionList)
        {
            this._entityVariantDefinitions = new Collection<EntityVariantDefinition>(entityVariantDefinitionList);
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public EntityVariantDefinitionCollection(String valueAsXml)
        {
            LoadEntityVariantDefinitionCollection(valueAsXml);
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
                return ObjectType.EntityVariantDefinition;
            }
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<EntityVariantDefinition> GetInternalCollection()
        {
            return _entityVariantDefinitions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityVariantDefinitionId"></param>
        /// <returns></returns>
        public EntityVariantDefinition Get(Int32 entityVariantDefinitionId)
        {
            return GetEntityVariantDefinition(entityVariantDefinitionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityVariantDefinitionName"></param>
        /// <returns></returns>
        public EntityVariantDefinition Get(String entityVariantDefinitionName)
        {
            return GetEntityVariantDefinition(entityVariantDefinitionName);
        }

        ///// <summary>
        ///// Clone EntityVariantDefinition collection.
        ///// </summary>
        ///// <returns>cloned EntityVariantDefinition collection object.</returns>
        //public IEntityVariantDefinitionCollection Clone()
        //{
        //    EntityVariantDefinitionCollection clonedEntityVariantDefinitions = new EntityVariantDefinitionCollection();

        //    if (this._entityVariantDefinitions != null && this._entityVariantDefinitions.Count > 0)
        //    {
        //        foreach (EntityVariantDefinition entityVariantDefinition in this._entityVariantDefinitions)
        //        {
        //            IEntityVariantDefinition clonedIEntityVariantDefinition = entityVariantDefinition.Clone();
        //            clonedEntityVariantDefinitions.Add(clonedIEntityVariantDefinition);
        //        }
        //    }

        //    return clonedEntityVariantDefinitions;
        //}

        /// <summary>
        /// Check if EntityVariantDefinitionCollection contains entity variant definition with given Id
        /// </summary>
        /// <param name="entityVariantDefinitionId">Id using which entity variant definition is to be searched from collection</param>
        /// <returns>
        /// <para>true : If entity variant definition found in EntityVariantDefinitionCollection</para>
        /// <para>false : If entity variant definition found not in EntityVariantDefinitionCollection</para>
        /// </returns>
        public bool Contains(Int32 entityVariantDefinitionId)
        {
            if (GetEntityVariantDefinition(entityVariantDefinitionId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entity variant definition object from EntityVariantDefinitionCollection
        /// </summary>
        /// <param name="entityVariantDefinitionId">Id of entity variant definition which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 entityVariantDefinitionId)
        {
            EntityVariantDefinition entityVariantDefinition = GetEntityVariantDefinition(entityVariantDefinitionId);

            if (entityVariantDefinition == null)
                throw new ArgumentException("No entity variant definition found for given entityVariantDefinitionId");
            else
                return this.Remove(entityVariantDefinition);
        }

        /// <summary>
        /// Get Xml representation of entity variant definition collection
        /// </summary>
        /// <returns>Xml representation of entity variant definition collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntityVariantDefinitions>";

            if (this._entityVariantDefinitions != null && this._entityVariantDefinitions.Count > 0)
            {
                foreach (EntityVariantDefinition definition in this._entityVariantDefinitions)
                {
                    returnXml = String.Concat(returnXml, definition.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityVariantDefinitions>");

            return returnXml;
        }

        /// <summary>
        /// Gets a cloned instance of the current entity variant definition collection object
        /// </summary>
        /// <returns>Returns a cloned instance of the current entity variant definition collection object</returns>
        public IEntityVariantDefinitionCollection Clone()
        {
            EntityVariantDefinitionCollection clonedEntityVariantDefinitions = new EntityVariantDefinitionCollection();

            if (this._entityVariantDefinitions != null && this._entityVariantDefinitions.Count > 0)
            {
                foreach (EntityVariantDefinition entityVariantDefinition in this._entityVariantDefinitions)
                {
                    EntityVariantDefinition clonedIEntityVariantDefinition = (EntityVariantDefinition)entityVariantDefinition.Clone();
                    clonedEntityVariantDefinitions.Add(clonedIEntityVariantDefinition);
                }
            }

            return clonedEntityVariantDefinitions;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityVariantDefinitionCollection)
            {
                EntityVariantDefinitionCollection objectToBeCompared = obj as EntityVariantDefinitionCollection;

                Int32 entityVariantDefinitionsUnion = this._entityVariantDefinitions.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityVariantDefinitionsIntersect = this._entityVariantDefinitions.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityVariantDefinitionsUnion != entityVariantDefinitionsIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for entity variant definition collection
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (EntityVariantDefinition entityVariantDefinition in this._entityVariantDefinitions)
            {
                hashCode += entityVariantDefinition.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region ICollection<EntityVariantDefinition> Members

        /// <summary>
        /// Add entity variant definition object in collection
        /// </summary>
        /// <param name="entityVariantDefinition">entity variant definition to add in collection</param>
        public void Add(EntityVariantDefinition entityVariantDefinition)
        {
            this._entityVariantDefinitions.Add(entityVariantDefinition);
        }

        /// <summary>
        /// Add entity variant definition object in collection
        /// </summary>
        /// <param name="entityVariantDefinition">entity variant definition to add in collection</param>
        public void Add(IEntityVariantDefinition entityVariantDefinition)
        {
            if (entityVariantDefinition != null)
            {
                Add((EntityVariantDefinition)entityVariantDefinition);
            }
        }

        /// <summary>
        /// Adds passed Entity Variant Definitions to the current collection
        /// </summary>
        /// <param name="entityVariantDefinitions">Collection of Entity Variant Definitions which needs to be added</param>
        public void AddRange(EntityVariantDefinitionCollection entityVariantDefinitions)
        {
            if (entityVariantDefinitions != null && entityVariantDefinitions.Count > 0)
            {
                foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
                {
                    this.Add(entityVariantDefinition);
                }
            }
        }

        /// <summary>
        /// Removes all entity variant definition from collection
        /// </summary>
        public void Clear()
        {
            this._entityVariantDefinitions.Clear();
        }

        /// <summary>
        /// Determines whether the EntityVariantDefinitionCollection contains a specific entity variant definition
        /// </summary>
        /// <param name="entityVariantDefinition">The entity variant definition object to locate in the EntityVariantDefinitionCollection.</param>
        /// <returns>
        /// <para>true : If entity variant definition found in EntityVariantDefinitionCollection</para>
        /// <para>false : If entity variant definition found not in EntityVariantDefinitionCollection</para>
        /// </returns>
        public bool Contains(EntityVariantDefinition entityVariantDefinition)
        {
            return this._entityVariantDefinitions.Contains(entityVariantDefinition);
        }

        /// <summary>
        /// Copies the elements of the EntityVariantDefinitionCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityVariantDefinition[] array, int arrayIndex)
        {
            this._entityVariantDefinitions.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entity variant definitions in EntityVariantDefinitionCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityVariantDefinitions.Count;
            }
        }

        /// <summary>
        /// Check if EntityVariantDefinitionCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity variant definition from the EntityVariantDefinitionCollection.
        /// </summary>
        /// <param name="entityVariantDefinition">The entity variant definition object to remove from the EntityVariantDefinitionCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntityVariantDefinition entityVariantDefinition)
        {
            return this._entityVariantDefinitions.Remove(entityVariantDefinition);
        }

        #endregion

        #region IEnumerable<EntityVariantDefinition> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityVariantDefinitionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityVariantDefinition> GetEnumerator()
        {
            return this._entityVariantDefinitions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityVariantDefinitionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityVariantDefinitions.GetEnumerator();
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
            Collection<IDataModelObjectCollection> entityVariantDefinitionsInBatch = null;

            if (this._entityVariantDefinitions != null)
            {
                entityVariantDefinitionsInBatch = Utility.Split(this, batchSize);
            }

            return entityVariantDefinitionsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as EntityVariantDefinition);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetEntityVariantDefinitionCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>        
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityVariantDefinitionCollection subsetEntityVariantDefinitionCollection, Boolean compareIds = false)
        {
            if (subsetEntityVariantDefinitionCollection != null && subsetEntityVariantDefinitionCollection.Count > 0)
            {
                foreach (EntityVariantDefinition entityVariantDefinition in subsetEntityVariantDefinitionCollection)
                {
                    //Get subset entity variant definition from super entity variant definition collection.
                    EntityVariantDefinition sourceEntityVariantDefinition = this.Get(entityVariantDefinition.Name);

                    if (sourceEntityVariantDefinition != null)
                    {
                        if (!sourceEntityVariantDefinition.IsSuperSetOf(entityVariantDefinition, compareIds))
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
        /// <param name="referenceIds">Indicates collection of referenceId of an entity variant definition which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            EntityVariantDefinitionCollection entityVariantDefinitions = GetEntityVariantDefinitions(referenceIds);

            if (entityVariantDefinitions != null && entityVariantDefinitions.Count > 0)
            {
                foreach (EntityVariantDefinition entityVariantDefinition in entityVariantDefinitions)
                {
                    result = result && this.Remove(entityVariantDefinition);
                }
            }

            return result;
        }

        /// <summary>
        /// Get entity variant definition by name
        /// </summary>
        /// <param name="entityVariantDefinitionName">Indicates entity variant definition name.</param>
        /// <returns>Returns entity variant definition for given name.</returns>
        public EntityVariantDefinition GetByName(String entityVariantDefinitionName)
        {
            if (!String.IsNullOrWhiteSpace(entityVariantDefinitionName) && this._entityVariantDefinitions.Count() > 0)
            {
                foreach (EntityVariantDefinition entityVatiantDefinition in this._entityVariantDefinitions)
                {
                    if (String.Compare(entityVatiantDefinition.Name, entityVariantDefinitionName) == 0)
                    {
                        return entityVatiantDefinition;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get entity variant definition by identifier
        /// </summary>
        /// <param name="entityVariantDefinitionId">Indicates identifier of entity variamt definition.</param>
        /// <returns>Returns entity variant definition for given identifier.</returns>
        public EntityVariantDefinition GetById(Int32 entityVariantDefinitionId)
        {
            if (entityVariantDefinitionId > 0)
            {
                foreach (EntityVariantDefinition entityVatiantDefinition in this._entityVariantDefinitions)
                {
                    if (entityVatiantDefinition.Id == entityVariantDefinitionId)
                    {
                        return entityVatiantDefinition;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize EntityVariantDefinitionCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">>Xml having value for EntityVariantDefinitionCollection</param>
        private void LoadEntityVariantDefinitionCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityVariantDefinition")
                        {
                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                EntityVariantDefinition entityVariantDefinition = new EntityVariantDefinition(attributeXml);
                                if (entityVariantDefinition != null)
                                {
                                    this.Add(entityVariantDefinition);
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
        /// <param name="entityVariantDefinitionId"></param>
        /// <returns></returns>
        private EntityVariantDefinition GetEntityVariantDefinition(Int32 entityVariantDefinitionId)
        {
            var filteredentityVariantDefinitions = from entityVariantDefinition in this._entityVariantDefinitions
                                                   where entityVariantDefinition.Id == entityVariantDefinitionId
                                                   select entityVariantDefinition;

            if (filteredentityVariantDefinitions.Any())
                return filteredentityVariantDefinitions.First();
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityVariantDefinitionName"></param>
        /// <returns></returns>
        private EntityVariantDefinition GetEntityVariantDefinition(String entityVariantDefinitionName)
        {
            entityVariantDefinitionName = entityVariantDefinitionName.ToLowerInvariant();

            var filteredentityVariantDefinitions = from entityVariantDefinition in this._entityVariantDefinitions
                                                   where entityVariantDefinition.Name.ToLowerInvariant().Equals(entityVariantDefinitionName)
                                                   select entityVariantDefinition;

            if (filteredentityVariantDefinitions.Any())
                return filteredentityVariantDefinitions.First();
            else
                return null;
        }

        /// <summary>
        ///  Gets the entity variant definition using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity variant definition which is to be fetched.</param>
        /// <returns>Returns filtered entity variant definitions</returns>
        private EntityVariantDefinitionCollection GetEntityVariantDefinitions(Collection<String> referenceIds)
        {
            EntityVariantDefinitionCollection entityVariantDefinitions = new EntityVariantDefinitionCollection();
            Int32 counter = 0;

            if (this._entityVariantDefinitions != null && this._entityVariantDefinitions.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (EntityVariantDefinition entityVariantDefinition in this._entityVariantDefinitions)
                {
                    if (referenceIds.Contains(entityVariantDefinition.ReferenceId))
                    {
                        entityVariantDefinitions.Add(entityVariantDefinition);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return entityVariantDefinitions;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current applicationcontext collection object
        /// </summary>
        /// <returns>Cloned instance of the current applicationcontext collection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #endregion
    }
}