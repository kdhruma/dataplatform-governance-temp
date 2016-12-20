using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Represents a collection of the entity cache status. 
    /// </summary>
    public class EntityCacheStatusCollection : ICollection<EntityCacheStatus>, IEnumerable<EntityCacheStatus>
    {
        #region Fields

        /// <summary>
        /// Holds a collection of entity cache status. 
        /// </summary>
        private Dictionary<String, EntityCacheStatus> _entityCacheStatusCollectionDictionary = new Dictionary<String, EntityCacheStatus>();

        private String _cacheStatusKeyFormat = "Entity-{0}_Container-{1}";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityCacheStatusCollection () : base() 
        { 
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public EntityCacheStatusCollection (String valueAsXml)
        {
            LoadEntityCacheStatusCollection (valueAsXml);
        }

        /// <summary>
        /// Initialize EntityCacheStatusCollection  from IList
        /// </summary>
        /// <param name="entityCacheStatusList">IList of EntityCacheStatus</param>
        public EntityCacheStatusCollection(IList<EntityCacheStatus> entityCacheStatusList)
        {
            if (entityCacheStatusList != null)
            {
                entityCacheStatusList.ToList().ForEach((x) =>
                    {
                        String key = GetCacheStatusKey(x.EntityId, x.ContainerId);
                        if (!this._entityCacheStatusCollectionDictionary.ContainsKey(key))
                        {
                            this._entityCacheStatusCollectionDictionary.Add(key, x);
                        }
                    }
                );
            }
        }

        #endregion

        #region Properties 
        #endregion

        #region Public Methods

        /// <summary>
        /// Check if EntityCacheStatusCollection  contains entityCacheStatus with given entityId
        /// </summary>
        /// <param name="entityId">entityId to search in EntityCacheStatusCollection </param>
        /// <param name="containerId">containerId to search in EntityCacheStatusCollection </param>
        /// <returns>
        /// <para>true : If entityCacheStatus found in EntityCacheStatusCollection </para>
        /// <para>false : If entityCacheStatus found not in EntityCacheStatusCollection </para>
        /// </returns>
        public bool Contains(Int64 entityId, Int32 containerId)
        {
            if (Get(entityId, containerId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entityCacheStatus object from EntityCacheStatusCollection 
        /// </summary>
        /// <param name="entityId">Id of entityCacheStatus which is to be removed from collection</param>
        /// <param name="containerId">containerId of entityCacheStatus which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int64 entityId, Int32 containerId)
        {
            EntityCacheStatus entityCacheStatus = Get(entityId, containerId);

            if (entityCacheStatus == null)
                throw new ArgumentException("No entityCacheStatus found for given entityCacheStatus id and container id");
            else
                return this.Remove(entityCacheStatus);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityCacheStatusCollection)
            {
                EntityCacheStatusCollection objectToBeCompared = obj as EntityCacheStatusCollection;
                Int32 entityCacheStatusUnion = this._entityCacheStatusCollectionDictionary.Values.Union(objectToBeCompared.ToList()).Count();
                Int32 entityCacheStatusIntersect = this._entityCacheStatusCollectionDictionary.Values.Intersect(objectToBeCompared.ToList()).Count();
                if (entityCacheStatusUnion != entityCacheStatusIntersect)
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
            foreach (EntityCacheStatus entityCacheStatus in this._entityCacheStatusCollectionDictionary.Values)
            {
                hashCode += entityCacheStatus.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Initialize EntityCacheStatusCollection  from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for EntityCacheStatusCollection 
        /// </param>
        public void LoadEntityCacheStatusCollection(String valuesAsXml)
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
                            String entityCacheStatusXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityCacheStatusXml))
                            {
                                EntityCacheStatus entityCacheStatus = new EntityCacheStatus(entityCacheStatusXml);
                                if (entityCacheStatus != null)
                                {
                                    this.Add(entityCacheStatus);
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
        /// Specifies if the cache status for any of the entities in the collection is updated.
        /// </summary>
        /// <returns></returns>
        public Boolean IsCacheStatusUpdated()
        {
            foreach (EntityCacheStatus entityCacheStatus in _entityCacheStatusCollectionDictionary.Values)
            {
                if (entityCacheStatus.IsCacheStatusUpdated)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the EntityCacheStatus, if it is available in the collection. Else it creates a record, adds it to the collection and returns the same.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public EntityCacheStatus GetOrCreateEntityCacheStatus(Int64 entityId, Int32 containerId)
        {
            EntityCacheStatus entityCacheStatus = this.GetEntityCacheStatus(entityId, containerId);
            if (entityCacheStatus == null)
            {
                entityCacheStatus = new EntityCacheStatus();
                entityCacheStatus.EntityId = entityId;
                entityCacheStatus.ContainerId = containerId;
                this.Add(entityCacheStatus);
            }
            return entityCacheStatus;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get EntityCacheStatus from current entityCacheStatus collection based on entityId. 
        /// </summary>
        /// <param name="entityId">Id of Entity for which CacheStatus is to be searched</param>
        /// <param name="containerId">Id of Entity for which CacheStatus is to be searched</param>
        /// <returns>EntityCacheStatus having given entityId </returns>
        private EntityCacheStatus Get(Int64 entityId, Int32 containerId)
        {
            EntityCacheStatus entityCacheStatus = null;

            _entityCacheStatusCollectionDictionary.TryGetValue(GetCacheStatusKey(entityId, containerId), out entityCacheStatus);
           
            return entityCacheStatus;
        }

        private String GetCacheStatusKey(Int64 entityId, Int32 containerId)
        {
            return String.Format(_cacheStatusKeyFormat, entityId, containerId < 1 ? 0 : containerId);
        }

        #endregion

        #region ICollection<EntityCacheStatus> Members

        /// <summary>
        /// Add entityCacheStatus object in collection
        /// </summary>
        /// <param name="item">entityCacheStatus to add in collection</param>
        public void Add(EntityCacheStatus item)
        {
            if (item != null)
            {
                String key = GetCacheStatusKey(item.EntityId, item.ContainerId);

                if (!this._entityCacheStatusCollectionDictionary.ContainsKey(key))
                {
                    this._entityCacheStatusCollectionDictionary.Add(key, item);
                }
            }
        }

        /// <summary>
        /// Add entityCacheStatus object in collection
        /// </summary>
        /// <param name="items">collection of entityCacheStatus to add in another collection</param>
        public void AddRange(EntityCacheStatusCollection items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("EntityCacheStatus");
            }

            foreach (EntityCacheStatus entityCacheStatus in items)
            {
                this.Add(entityCacheStatus);
            }
        }

        /// <summary>
        /// Removes all entityCacheStatus from collection
        /// </summary>
        public void Clear()
        {
            this._entityCacheStatusCollectionDictionary.Clear();
        }

        /// <summary>
        /// Determines whether the EntityCacheStatusCollection  contains a specific entityCacheStatus.
        /// </summary>
        /// <param name="item">The entityCacheStatus object to locate in the EntityCacheStatusCollection .</param>
        /// <returns>
        /// <para>true : If entityCacheStatus found in EntityCacheStatusCollection </para>
        /// <para>false : If entityCacheStatus found not in EntityCacheStatusCollection </para>
        /// </returns>
        public bool Contains(EntityCacheStatus item)
        {
            return this._entityCacheStatusCollectionDictionary.ContainsKey(GetCacheStatusKey(item.EntityId, item.ContainerId));
        }

        /// <summary>
        /// Copies the elements of the EntityCacheStatusCollection  to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityCacheStatusCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityCacheStatus[] array, int arrayIndex)
        {
            this._entityCacheStatusCollectionDictionary.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entityCacheStatus in EntityCacheStatusCollection 
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityCacheStatusCollectionDictionary.Count;
            }
        }

        /// <summary>
        /// Check if EntityCacheStatusCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityCacheStatusCollection.
        /// </summary>
        /// <param name="item">The entityCacheStatus object to remove from the EntityCacheStatusCollection .</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityCacheStatusCollection </returns>
        public bool Remove(EntityCacheStatus item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("EntityCacheStatus");
            }

            return this._entityCacheStatusCollectionDictionary.Remove(GetCacheStatusKey(item.EntityId, item.ContainerId));
        }

        #endregion

        #region IEnumerable<EntityCacheStatus> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCacheStatusCollection .
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityCacheStatus> GetEnumerator()
        {
            return this._entityCacheStatusCollectionDictionary.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityCacheStatusCollection .
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityCacheStatusCollectionDictionary.Values.GetEnumerator();
        }

        #endregion

        #region IEntityCacheStatusCollection  Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of EntityCacheStatusCollection  object
        /// </summary>
        /// <returns>Xml string representing the EntityCacheStatusCollection </returns>
        public String ToXml()
        {
            String entityCacheStatusXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityCacheStatus entityCacheStatus in this._entityCacheStatusCollectionDictionary.Values)
            {
                builder.Append(entityCacheStatus.ToXml());
            }

            entityCacheStatusXml = String.Format("<EntityCacheStatusCollection>{0}</EntityCacheStatusCollection>", builder.ToString());
            return entityCacheStatusXml;
        }

        /// <summary>
        /// Get Xml representation of EntityCacheStatusCollection  object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String entityCacheStatusXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityCacheStatus entityCacheStatus in this._entityCacheStatusCollectionDictionary.Values)
            {
                builder.Append(entityCacheStatus.ToXml(serialization));
            }

            entityCacheStatusXml = String.Format("<EntityCacheStatusCollection>{0}</EntityCacheStatusCollection>", builder.ToString());
            return entityCacheStatusXml;
        }

        #endregion ToXml methods

        #region EntityCacheStatus Get

        /// <summary>
        /// Gets EntityCacheStatus with specified entity Id.
        /// </summary>
        /// <param name="entityId">Id of an entityCacheStatus to search in current entity's entityCacheStatus</param>
        /// <param name="containerId">containerId of an entityCacheStatus to search in current entity's entityCacheStatus</param>
        /// <returns>EntityCacheStatus object</returns>
        /// <exception cref="ArgumentException">Entity Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no items to search in</exception>
        /// <exception cref="MDMOperationException">Not able to find appropriate entityCacheStatus as entityCacheStatus is localized. Please provide locale details.</exception>
        public EntityCacheStatus GetEntityCacheStatus(Int64 entityId, Int32 containerId)
        {
            if (this._entityCacheStatusCollectionDictionary == null)
            {
                throw new NullReferenceException("There are no entityCacheStatus collections to search in");
            }

            if (entityId <= 0)
            {
                throw new ArgumentException("Entity Id must be greater than 0", entityId.ToString());
            }

            EntityCacheStatus entityCacheStatus = Get(entityId, containerId);

            return entityCacheStatus;
        }

        #endregion

        #endregion IEntityCacheStatusCollection  Memebers
    }
}
