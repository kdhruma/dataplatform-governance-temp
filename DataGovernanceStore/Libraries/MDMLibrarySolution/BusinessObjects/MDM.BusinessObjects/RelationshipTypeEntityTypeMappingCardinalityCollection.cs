using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.BusinessObjects.Interfaces;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping collection object of RelationshipType EntityType Cardinality mapping
    /// </summary>
    [DataContract]
    public class RelationshipTypeEntityTypeMappingCardinalityCollection : ICollection<RelationshipTypeEntityTypeMappingCardinality>, IEnumerable<RelationshipTypeEntityTypeMappingCardinality>, IRelationshipTypeEntityTypeMappingCardinalityCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<RelationshipTypeEntityTypeMappingCardinality> _relationshipTypeEntityTypeMappingCardinalitys = new Collection<RelationshipTypeEntityTypeMappingCardinality>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public RelationshipTypeEntityTypeMappingCardinalityCollection() : base() { }

        /// <summary>
        /// Initialize RelationshipTypeEntityTypeMappingCardinalityCollection from IList
        /// </summary>
        /// <param name="relationshipTypeEntityTypeMappingCardinalitys">IList of RelationshipTypeEntityTypeMappingCardinality</param>
        public RelationshipTypeEntityTypeMappingCardinalityCollection(IList<RelationshipTypeEntityTypeMappingCardinality> relationshipTypeEntityTypeMappingCardinalitys)
        {
            this._relationshipTypeEntityTypeMappingCardinalitys = new Collection<RelationshipTypeEntityTypeMappingCardinality>(relationshipTypeEntityTypeMappingCardinalitys);
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
                return ObjectType.RelationshipTypeEntityTypeMappingCardinality;
            }
        }

        #endregion

        #region Method

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Gets the RelationshipTypeEntityTypeMappingCardinality for given EntityTypeName, RelationshipTypeName and Related EntityTypeName.
        /// </summary>
        /// <param name="entityTypeName">EntityTypeName to be searched in the collection</param>
        /// <param name="relationshipTypeName">RelationshipTypeName to be searched in the collection</param>
        /// <param name="toEntityTypeName">Related EntityType Name to be searched in the collection</param>
        /// <returns>RelationshipTypeEntityTypeMappingCardinality for given EntityTypeName, RelationshipTypeName and Related EntityTypeName</returns>
        public RelationshipTypeEntityTypeMappingCardinality Get(String entityTypeName, String relationshipTypeName, String toEntityTypeName)
        {
            if (this._relationshipTypeEntityTypeMappingCardinalitys != null && this._relationshipTypeEntityTypeMappingCardinalitys.Count > 0)
            {
                entityTypeName = entityTypeName.ToLowerInvariant();
                relationshipTypeName = relationshipTypeName.ToLowerInvariant();
                toEntityTypeName = toEntityTypeName.ToLowerInvariant();

                foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in this._relationshipTypeEntityTypeMappingCardinalitys)
                {
                    if (relationshipTypeEntityTypeMappingCardinality.EntityTypeName.ToLowerInvariant().Equals(entityTypeName) &&
                        relationshipTypeEntityTypeMappingCardinality.RelationshipTypeName.ToLowerInvariant().Equals(relationshipTypeName) &&
                        relationshipTypeEntityTypeMappingCardinality.ToEntityTypeName.ToLowerInvariant().Equals(toEntityTypeName))
                    {
                        return relationshipTypeEntityTypeMappingCardinality;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RelationshipTypeEntityTypeMappingCardinalityCollection)
            {
                RelationshipTypeEntityTypeMappingCardinalityCollection objectToBeCompared = obj as RelationshipTypeEntityTypeMappingCardinalityCollection;
                Int32 RelationshipTypeEntityTypeMappingCardinalitysUnion = this._relationshipTypeEntityTypeMappingCardinalitys.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 RelationshipTypeEntityTypeMappingCardinalitysIntersect = this._relationshipTypeEntityTypeMappingCardinalitys.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (RelationshipTypeEntityTypeMappingCardinalitysUnion != RelationshipTypeEntityTypeMappingCardinalitysIntersect)
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
            foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in this._relationshipTypeEntityTypeMappingCardinalitys)
            {
                hashCode += relationshipTypeEntityTypeMappingCardinality.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region ICollection<RelationshipTypeEntityTypeMappingCardinality> Members

        /// <summary>
        /// Add RelationshipTypeEntityTypeMappingCardinality object in collection
        /// </summary>
        /// <param name="item">RelationshipTypeEntityTypeMappingCardinality to add in collection</param>
        public void Add(RelationshipTypeEntityTypeMappingCardinality item)
        {
            this._relationshipTypeEntityTypeMappingCardinalitys.Add(item);
        }

        /// <summary>
        /// Add IRelationshipTypeEntityTypeMappingCardinality in collection
        /// </summary>
        /// <param name="item">IRelationshipTypeEntityTypeMappingCardinality to add in collection</param>
        public void Add(IRelationshipTypeEntityTypeMappingCardinality item)
        {
            if (item != null)
            {
                this.Add((RelationshipTypeEntityTypeMappingCardinality)item);
            }
        }

        /// <summary>
        /// Add RelationshipTypeEntityTypeMappingCardinality object in collection
        /// </summary>
        /// <param name="items">RelationshipTypeEntityTypeMappingCardinality to add in collection</param>
        public void AddRange(RelationshipTypeEntityTypeMappingCardinalityCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMappingCardinality item in items)
                {
                    this._relationshipTypeEntityTypeMappingCardinalitys.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes all RelationshipTypeEntityTypeMappingCardinalitys from collection
        /// </summary>
        public void Clear()
        {
            this._relationshipTypeEntityTypeMappingCardinalitys.Clear();
        }

        /// <summary>
        /// Determines whether the RelationshipTypeEntityTypeMappingCardinalityCollection contains a specific RelationshipTypeEntityTypeMappingCardinality.
        /// </summary>
        /// <param name="item">The RelationshipTypeEntityTypeMappingCardinality object to locate in the RelationshipTypeEntityTypeMappingCardinalityCollection.</param>
        /// <returns>
        /// <para>true : If RelationshipTypeEntityTypeMappingCardinality found in mappingCollection</para>
        /// <para>false : If RelationshipTypeEntityTypeMappingCardinality found not in mappingCollection</para>
        /// </returns>
        public bool Contains(RelationshipTypeEntityTypeMappingCardinality item)
        {
            return this._relationshipTypeEntityTypeMappingCardinalitys.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the RelationshipTypeEntityTypeMappingCardinalityCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from RelationshipTypeEntityTypeMappingCardinalityCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(RelationshipTypeEntityTypeMappingCardinality[] array, int arrayIndex)
        {
            this._relationshipTypeEntityTypeMappingCardinalitys.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of RelationshipTypeEntityTypeMappingCardinality in RelationshipTypeEntityTypeMappingCardinalityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._relationshipTypeEntityTypeMappingCardinalitys.Count;
            }
        }

        /// <summary>
        /// Check if RelationshipTypeEntityTypeMappingCardinalityCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the RelationshipTypeEntityTypeMappingCardinalityCollection.
        /// </summary>
        /// <param name="item">The RelationshipTypeEntityTypeMappingCardinality object to remove from the RelationshipTypeEntityTypeMappingCardinalityCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original RelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        public bool Remove(RelationshipTypeEntityTypeMappingCardinality item)
        {
            return this._relationshipTypeEntityTypeMappingCardinalitys.Remove(item);
        }

        #endregion

        #region IEnumerable<RelationshipTypeEntityTypeMappingCardinality> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeEntityTypeMappingCardinalityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<RelationshipTypeEntityTypeMappingCardinality> GetEnumerator()
        {
            return this._relationshipTypeEntityTypeMappingCardinalitys.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipTypeEntityTypeMappingCardinalityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._relationshipTypeEntityTypeMappingCardinalitys.GetEnumerator();
        }

        #endregion

        #region IRelationshipTypeEntityTypeMappingCollection Members

        /// <summary>
        /// Get Xml representation of RelationshipTypeEntityTypeMappingCardinalityCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        public String ToXml()
        {
            String relationshipTypeEntityTypeMappingCardinalitysXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in this._relationshipTypeEntityTypeMappingCardinalitys)
            {
                builder.Append(relationshipTypeEntityTypeMappingCardinality.ToXML());
            }

            relationshipTypeEntityTypeMappingCardinalitysXml = String.Format("<RelationshipTypeEntityTypeMappingCardinalitys>{0}</RelationshipTypeEntityTypeMappingCardinalitys>", builder.ToString());

            return relationshipTypeEntityTypeMappingCardinalitysXml;
        }

        #endregion IRelationshipTypeEntityTypeMappingCollection Members

        #region IDataModelObjectCollection

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = Get(referenceIds);

            if (relationshipTypeEntityTypeMappingCardinalitys != null && relationshipTypeEntityTypeMappingCardinalitys.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in relationshipTypeEntityTypeMappingCardinalitys)
                {
                    result = result && this.Remove(relationshipTypeEntityTypeMappingCardinality);
                }
            }

            return result;
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> relationshipTypeEntityTypeMappingCardinalitiesInBatch = null;

            if (this._relationshipTypeEntityTypeMappingCardinalitys != null)
            {
                relationshipTypeEntityTypeMappingCardinalitiesInBatch = Utility.Split(this, batchSize);
            }

            return relationshipTypeEntityTypeMappingCardinalitiesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as RelationshipTypeEntityTypeMappingCardinality);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the RelationshipTypeEntityTypeMappingCardinalityCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>RelationshipTypeEntityTypeMappingCardinalityCollection having given referenceIds</returns>
        private RelationshipTypeEntityTypeMappingCardinalityCollection Get(Collection<String> referenceIds)
        {
            RelationshipTypeEntityTypeMappingCardinalityCollection relationshipTypeEntityTypeMappingCardinalitys = new RelationshipTypeEntityTypeMappingCardinalityCollection();
            Int32 counter = 0;

            if (this._relationshipTypeEntityTypeMappingCardinalitys != null && this._relationshipTypeEntityTypeMappingCardinalitys.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (RelationshipTypeEntityTypeMappingCardinality relationshipTypeEntityTypeMappingCardinality in this._relationshipTypeEntityTypeMappingCardinalitys)
                {
                    if (referenceIds.Contains(relationshipTypeEntityTypeMappingCardinality.ReferenceId))
                    {
                        relationshipTypeEntityTypeMappingCardinalitys.Add(relationshipTypeEntityTypeMappingCardinality);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }
            return relationshipTypeEntityTypeMappingCardinalitys;
        }

        #endregion

        #endregion
    }
}