using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Collection for entity activity log item status for data quality management system
    /// </summary>
    [DataContract]
    public class EntityActivityLogItemStatusCollection : IEntityActivityLogItemStatusCollection
    {
         #region Fields

        [DataMember]
        private Collection<EntityActivityLogItemStatus> _entityActivityLogItemStatusCollection = new Collection<EntityActivityLogItemStatus>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityActivityLogItemStatusCollection() { }        

        /// <summary>
        /// Initialize EntityActivityLogItemStatusCollection from IList
        /// </summary>
        /// <param name="entityActivityLogStatusList">IList of entityActivityLogStatus</param>
        public EntityActivityLogItemStatusCollection(IList<EntityActivityLogItemStatus> entityActivityLogStatusList)
        {
            this._entityActivityLogItemStatusCollection = new Collection<EntityActivityLogItemStatus>(entityActivityLogStatusList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find EntityActivityLogItemStatus from EntityActivityLogItemStatusCollection based on entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">entityActivityLogId to search in result collection</param>
        /// <returns>EntityActivityLogItemStatus object having given entityActivityLogId</returns>
        public EntityActivityLogItemStatus this[Int64 entityActivityLogId]
        {
            get
            {
                EntityActivityLogItemStatus entityActivityLogItemStatus = GetEntityActivityLogStatus(entityActivityLogId);
                if (entityActivityLogItemStatus == null)
                    throw new ArgumentException(String.Format("No result found for EntityActivityLogId: {0}", entityActivityLogId), "Id");
                
                return entityActivityLogItemStatus;
            }
            set
            {
                EntityActivityLogItemStatus entityActivityLogItemStatus = GetEntityActivityLogStatus(entityActivityLogId);
                if (entityActivityLogItemStatus == null)
                    throw new ArgumentException(String.Format("No result found for  EntityActivityLogId: {0}", entityActivityLogId), "Id");

                entityActivityLogItemStatus = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the EntityActivityLogItemStatus based on the unique entityActivityLogId
        /// </summary>
        /// <param name="entityActivityLogId">pk of EntityActivityLogItemStatus</param>
        public EntityActivityLogItemStatus GetEntityActivityLogStatus(Int64 entityActivityLogId)
        {
            return _entityActivityLogItemStatusCollection.FirstOrDefault(x => x.EntityActivityLogId == entityActivityLogId);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityActivityLogItemStatusCollection)
            {
                EntityActivityLogItemStatusCollection objectToBeCompared = obj as EntityActivityLogItemStatusCollection;
                Int32 entityActivityLogUnion = this._entityActivityLogItemStatusCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityActivityLogIntersect = this._entityActivityLogItemStatusCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (entityActivityLogUnion != entityActivityLogIntersect)
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
            foreach (EntityActivityLogItemStatus attr in this._entityActivityLogItemStatusCollection)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }       
      
        #endregion

        #region Private Methods
       
        #endregion

        #region ICollection<EntityActivityLogItemStatus> Members

        /// <summary>
        /// Add EntityActivityLogItemStatus object in collection
        /// </summary>
        /// <param name="item">EntityActivityLogItemStatus to add in collection</param>
        public void Add(EntityActivityLogItemStatus item)
        {
            this._entityActivityLogItemStatusCollection.Add(item);
        }

        /// <summary>
        /// Removes all EntityActivityLogItemStatus from collection
        /// </summary>
        public void Clear()
        {
            this._entityActivityLogItemStatusCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityActivityLogItemStatusCollection contains a specific EntityActivityLogItemStatus.
        /// </summary>
        /// <param name="item">The EntityActivityLogItemStatus object to locate in the EntityActivityLogItemStatusCollection.</param>
        /// <returns>
        /// <para>true : If EntityActivityLogItemStatus found in mappingCollection</para>
        /// <para>false : If EntityActivityLogItemStatus found not in mappingCollection</para>
        /// </returns>
        public bool Contains(EntityActivityLogItemStatus item)
        {
            return this._entityActivityLogItemStatusCollection.Contains(item);
        }

        /// <summary>
        /// Determines whether the EntityActivityLogItemStatusCollection contains a specific entityActivityLogItemStatus based on EntityActivityLogId.
        /// </summary>
        /// <param name="entityActivityLogId">The EntityActivityLogId locate in the EntityActivityLogItemStatusCollection.</param>
        /// <returns>
        /// <para>true : If EntityActivityLogId found in mappingCollection</para>
        /// <para>false : If EntityActivityLogId found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Int64 entityActivityLogId)
        {
            return GetEntityActivityLogStatus(entityActivityLogId) != null;                
        }

        /// <summary>
        /// Copies the elements of the EntityActivityLogItemStatusCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityActivityLogItemStatusCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityActivityLogItemStatus[] array, int arrayIndex)
        {
            this._entityActivityLogItemStatusCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of EntityActivityLogItemStatus in EntityActivityLogItemStatusCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityActivityLogItemStatusCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityActivityLogItemStatusCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EntityActivityLogItemStatusCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the EntityActivityLogItemStatusCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original EntityActivityLogItemStatusCollection</returns>
        public bool Remove(EntityActivityLogItemStatus item)
        {
            return this._entityActivityLogItemStatusCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<EntityActivityLogItemStatus> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityActivityLogItemStatusCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityActivityLogItemStatus> GetEnumerator()
        {
            return this._entityActivityLogItemStatusCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityActivityLogItemStatusCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityActivityLogItemStatusCollection.GetEnumerator();
        }

        #endregion       
    }
}
