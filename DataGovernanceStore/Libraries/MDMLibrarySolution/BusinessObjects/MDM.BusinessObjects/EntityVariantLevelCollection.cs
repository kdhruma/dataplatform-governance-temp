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
    /// Specifies Entity Variant Level Collection
    /// </summary>
    [DataContract]
    public class EntityVariantLevelCollection : ICollection<EntityVariantLevel>, IEnumerable<EntityVariantLevel>, IEntityVariantLevelCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of entity variant Level
        /// </summary>
        [DataMember]
        private Collection<EntityVariantLevel> _entityVariantLevels = new Collection<EntityVariantLevel>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the entity variant Level collection
        /// </summary>
        public EntityVariantLevelCollection() : base() { }

        /// <summary>
        /// Initialize EntityVariantLevelCollection from IList of value
        /// </summary>
        /// <param name="entityVariantLevelList">List of EntityVariantLevel object</param>
        public EntityVariantLevelCollection(IList<EntityVariantLevel> entityVariantLevelList)
        {
            this._entityVariantLevels = new Collection<EntityVariantLevel>(entityVariantLevelList);
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public EntityVariantLevelCollection(String valueAsXml)
        {
            LoadEntityVariantLevelCollection(valueAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Utility methods
        
        /// <summary>
        /// Check if EntityVariantLevelCollection contains entity variant Level with given Id
        /// </summary>
        /// <param name="entityVariantLevelId">Id using which entity variant Level is to be searched from collection</param>
        /// <returns>
        /// <para>true : If entity variant Level found in EntityVariantLevelCollection</para>
        /// <para>false : If entity variant Level found not in EntityVariantLevelCollection</para>
        /// </returns>
        public bool Contains(Int32 entityVariantLevelId)
        {
            if (GetEntityVariantLevel(entityVariantLevelId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entity variant Level object from EntityVariantLevelCollection
        /// </summary>
        /// <param name="entityVariantLevelId">Id of entity variant Level which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 entityVariantLevelId)
        {
            EntityVariantLevel entityVariantLevel = GetEntityVariantLevel(entityVariantLevelId);

            if (entityVariantLevel == null)
                throw new ArgumentException("No entity variant Level found for given entityVariantLevelId");
            else
                return this.Remove(entityVariantLevel);
        }

        /// <summary>
        /// Get Xml representation of entity variant Level collection
        /// </summary>
        /// <returns>Xml representation of entity variant Level collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntityVariantLevels>";

            if (this._entityVariantLevels != null && this._entityVariantLevels.Count > 0)
            {
                foreach (EntityVariantLevel Level in this._entityVariantLevels)
                {
                    returnXml = String.Concat(returnXml, Level.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityVariantLevels>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityVariantLevelCollection)
            {
                EntityVariantLevelCollection objectToBeCompared = obj as EntityVariantLevelCollection;

                Int32 entityVariantLevelsUnion = this._entityVariantLevels.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityVariantLevelsIntersect = this._entityVariantLevels.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityVariantLevelsUnion != entityVariantLevelsIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for entity variant Level collection
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (EntityVariantLevel entityVariantLevel in this._entityVariantLevels)
            {
                hashCode += entityVariantLevel.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region ICollection<EntityVariantLevel> Members

        /// <summary>
        /// Add entity variant Level object in collection
        /// </summary>
        /// <param name="entityVariantLevel">entity variant Level to add in collection</param>
        public void Add(EntityVariantLevel entityVariantLevel)
        {
            this._entityVariantLevels.Add(entityVariantLevel);
        }

        /// <summary>
        /// Add entity variant Level object in collection
        /// </summary>
        /// <param name="entityVariantLevel">entity variant Level to add in collection</param>
        public void Add(IEntityVariantLevel entityVariantLevel)
        {
            if (entityVariantLevel != null)
            {
                Add((EntityVariantLevel)entityVariantLevel);
            }
        }

        /// <summary>
        /// Adds passed Entity Variant Levels to the current collection
        /// </summary>
        /// <param name="entityVariantLevels">Collection of Entity Variant Levels which needs to be added</param>
        public void AddRange(EntityVariantLevelCollection entityVariantLevels)
        {
            if (entityVariantLevels != null && entityVariantLevels.Count > 0)
            {
                foreach (EntityVariantLevel entityVariantLevel in entityVariantLevels)
                {
                    this.Add(entityVariantLevel);
                }
            }
        }

        /// <summary>
        /// Removes all entity variant Level from collection
        /// </summary>
        public void Clear()
        {
            this._entityVariantLevels.Clear();
        }

        /// <summary>
        /// Determines whether the EntityVariantLevelCollection contains a specific entity variant Level
        /// </summary>
        /// <param name="entityVariantLevel">The entity variant Level object to locate in the EntityVariantLevelCollection.</param>
        /// <returns>
        /// <para>true : If entity variant Level found in EntityVariantLevelCollection</para>
        /// <para>false : If entity variant Level found not in EntityVariantLevelCollection</para>
        /// </returns>
        public bool Contains(EntityVariantLevel entityVariantLevel)
        {
            return this._entityVariantLevels.Contains(entityVariantLevel);
        }

        /// <summary>
        /// Copies the elements of the EntityVariantLevelCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityVariantLevel[] array, int arrayIndex)
        {
            this._entityVariantLevels.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entity variant Levels in EntityVariantLevelCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityVariantLevels.Count;
            }
        }

        /// <summary>
        /// Check if EntityVariantLevelCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity variant Level from the EntityVariantLevelCollection.
        /// </summary>
        /// <param name="entityVariantLevel">The entity variant Level object to remove from the EntityVariantLevelCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntityVariantLevel entityVariantLevel)
        {
            return this._entityVariantLevels.Remove(entityVariantLevel);
        }

        /// <summary>
        /// Get entity variant level by rank
        /// </summary>
        /// <param name="entityVariantLevelRank">Indicates rank of entity variant level</param>
        /// <returns>Returns entity variant level for given rank.</returns>
        public EntityVariantLevel GetByRank(Int32 entityVariantLevelRank)
        {
          if (entityVariantLevelRank > 0)
            {
                foreach (EntityVariantLevel entityVariantLevel in this._entityVariantLevels)
                {
                    if (entityVariantLevel.Rank == entityVariantLevelRank)
                    {
                        return entityVariantLevel;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get entity variant level by entity type
        /// </summary>
        /// <param name="entityTypeName">Indicates entity type of entity variant level</param>
        /// <returns>Returns entity variant level for given entity type</returns>
        public EntityVariantLevel GetByEntityType(String entityTypeName)
        {
            if (!String.IsNullOrWhiteSpace(entityTypeName))
            {
                foreach (EntityVariantLevel entityVariantLevel in this._entityVariantLevels)
                {
                    if (String.Compare(entityVariantLevel.EntityTypeName, entityTypeName, true) == 0)
                    {
                        return entityVariantLevel;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get entity variant level based on entity type identifier 
        /// </summary>
        /// <param name="entityTypeId">Indicates identifier of entity type.</param>
        /// <returns>Returns entity variant level based on given entity type identifier.</returns>
        public EntityVariantLevel GetByEntityTypeId(Int32 entityTypeId)
        {
            if (entityTypeId > 0)
            {
                foreach (EntityVariantLevel entityVariantLevel in this._entityVariantLevels)
                {
                    if (entityVariantLevel.EntityTypeId == entityTypeId)
                    {
                        return entityVariantLevel;
                    }
                }
            }

            return null;
        }

        #endregion

        #region IEnumerable<EntityVariantLevel> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityVariantLevelCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityVariantLevel> GetEnumerator()
        {
            return this._entityVariantLevels.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityVariantLevelCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityVariantLevels.GetEnumerator();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity variant Level which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            EntityVariantLevelCollection entityVariantLevels = GetEntityVariantLevels(referenceIds);

            if (entityVariantLevels != null && entityVariantLevels.Count > 0)
            {
                foreach (EntityVariantLevel entityVariantLevel in entityVariantLevels)
                {
                    result = result && this.Remove(entityVariantLevel);
                }
            }

            return result;
        }

        /// <summary>
        /// Clone entity variant level collection object
        /// </summary>
        /// <returns>Returns colned entity variant level collection instance</returns>
        public IEntityVariantLevelCollection Clone()
        {
            EntityVariantLevelCollection clonedEntityVariantLevelCollection = new EntityVariantLevelCollection();

            if (this._entityVariantLevels != null && this._entityVariantLevels.Count > 0)
            {
                foreach (EntityVariantLevel entityVariantLevel in this._entityVariantLevels)
                {
                    IEntityVariantLevel clonedIEntityVariantLevel = entityVariantLevel.Clone();
                    clonedEntityVariantLevelCollection.Add(clonedIEntityVariantLevel as EntityVariantLevel);
                }
            }

            return clonedEntityVariantLevelCollection;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize EntityVariantLevelCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">>Xml having value for EntityVariantLevelCollection</param>
        private void LoadEntityVariantLevelCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityVariantLevel")
                        {
                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                EntityVariantLevel entityVariantLevel = new EntityVariantLevel(attributeXml);
                                if (entityVariantLevel != null)
                                {
                                    this.Add(entityVariantLevel);
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

        private EntityVariantLevel GetEntityVariantLevel(Int32 entityVariantLevelId)
        {
            var filteredentityVariantLevels = from entityVariantLevel in this._entityVariantLevels
                                                   where entityVariantLevel.Id == entityVariantLevelId
                                                   select entityVariantLevel;

            if (filteredentityVariantLevels.Any())
                return filteredentityVariantLevels.First();
            else
                return null;
        }

        private EntityVariantLevel GetEntityVariantLevel(String entityVariantLevelName)
        {
            entityVariantLevelName = entityVariantLevelName.ToLowerInvariant();

            var filteredEntityVariantLevels = from entityVariantLevel in this._entityVariantLevels
                                                   where entityVariantLevel.Name.ToLowerInvariant().Equals(entityVariantLevelName)
                                                   select entityVariantLevel;

            if (filteredEntityVariantLevels.Any())
                return filteredEntityVariantLevels.First();
            else
                return null;
        }

        /// <summary>
        ///  Gets the entity variant Level using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity variant Level which is to be fetched.</param>
        /// <returns>Returns filtered entity variant Levels</returns>
        private EntityVariantLevelCollection GetEntityVariantLevels(Collection<String> referenceIds)
        {
            EntityVariantLevelCollection entityVariantLevels = new EntityVariantLevelCollection();
            Int32 counter = 0;

            if (this._entityVariantLevels != null && this._entityVariantLevels.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (EntityVariantLevel entityVariantLevel in this._entityVariantLevels)
                {
                    if (referenceIds.Contains(entityVariantLevel.ReferenceId))
                    {
                        entityVariantLevels.Add(entityVariantLevel);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return entityVariantLevels;
        }

        #endregion

        #endregion
    }
}