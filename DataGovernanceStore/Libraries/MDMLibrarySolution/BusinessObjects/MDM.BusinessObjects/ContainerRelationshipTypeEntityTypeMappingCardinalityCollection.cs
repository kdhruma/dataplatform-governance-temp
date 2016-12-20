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
    public class ContainerRelationshipTypeEntityTypeMappingCardinalityCollection : ICollection<ContainerRelationshipTypeEntityTypeMappingCardinality>, IEnumerable<ContainerRelationshipTypeEntityTypeMappingCardinality>, IContainerRelationshipTypeEntityTypeMappingCardinalityCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<ContainerRelationshipTypeEntityTypeMappingCardinality> _containerRelationshipTypeEntityTypeMappingCardinalitys = new Collection<ContainerRelationshipTypeEntityTypeMappingCardinality>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection() : base() { }

        /// <summary>
        /// Initialize ContainerRelationshipTypeEntityTypeMappingCardinalityCollection from IList
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalitys">IList of ContainerRelationshipTypeEntityTypeMappingCardinality</param>
        public ContainerRelationshipTypeEntityTypeMappingCardinalityCollection(IList<ContainerRelationshipTypeEntityTypeMappingCardinality> containerRelationshipTypeEntityTypeMappingCardinalitys)
        {
            this._containerRelationshipTypeEntityTypeMappingCardinalitys = new Collection<ContainerRelationshipTypeEntityTypeMappingCardinality>(containerRelationshipTypeEntityTypeMappingCardinalitys);
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
                return MDM.Core.ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality;
            }
        }

        #endregion

        #region Method

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Gets the ContainerRelationshipTypeEntityTypeMappingCardinality for given EntityTypeName, RelationshipTypeName and Related EntityTypeName.
        /// </summary>
        /// <param name="organizationName">OrganizationName to be searched in the collection</param>
        /// <param name="containerName">ContainerName to be searched in the collection</param>
        /// <param name="entityTypeName">EntityTypeName to be searched in the collection</param>
        /// <param name="relationshipTypeName">RelationshipTypeName to be searched in the collection</param>
        /// <param name="toEntityTypeName">Related EntityType Name to be searched in the collection</param>
        /// <returns>ContainerRelationshipTypeEntityTypeMappingCardinality for given EntityTypeName, RelationshipTypeName and Related EntityTypeName</returns>
        public ContainerRelationshipTypeEntityTypeMappingCardinality Get(String organizationName, String containerName, String entityTypeName, String relationshipTypeName, String toEntityTypeName)
        {
            if (this._containerRelationshipTypeEntityTypeMappingCardinalitys != null && this._containerRelationshipTypeEntityTypeMappingCardinalitys.Count > 0)
            {
                organizationName = organizationName.ToLowerInvariant();
                containerName = containerName.ToLowerInvariant();
                entityTypeName = entityTypeName.ToLowerInvariant();
                relationshipTypeName = relationshipTypeName.ToLowerInvariant();
                toEntityTypeName = toEntityTypeName.ToLowerInvariant();

                foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in this._containerRelationshipTypeEntityTypeMappingCardinalitys)
                {
                    if (containerRelationshipTypeEntityTypeMappingCardinality.OrganizationName.ToLowerInvariant().Equals(organizationName) &&
                        containerRelationshipTypeEntityTypeMappingCardinality.ContainerName.ToLowerInvariant().Equals(containerName) &&
                        containerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName.ToLowerInvariant().Equals(entityTypeName) &&
                        containerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName.ToLowerInvariant().Equals(relationshipTypeName) &&
                        containerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName.ToLowerInvariant().Equals(toEntityTypeName))
                    {
                        return containerRelationshipTypeEntityTypeMappingCardinality;
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
            if (obj is ContainerRelationshipTypeEntityTypeMappingCardinalityCollection)
            {
                ContainerRelationshipTypeEntityTypeMappingCardinalityCollection objectToBeCompared = obj as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection;
                Int32 ContainerRelationshipTypeEntityTypeMappingCardinalitysUnion = this._containerRelationshipTypeEntityTypeMappingCardinalitys.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 ContainerRelationshipTypeEntityTypeMappingCardinalitysIntersect = this._containerRelationshipTypeEntityTypeMappingCardinalitys.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (ContainerRelationshipTypeEntityTypeMappingCardinalitysUnion != ContainerRelationshipTypeEntityTypeMappingCardinalitysIntersect)
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
            foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in this._containerRelationshipTypeEntityTypeMappingCardinalitys)
            {
                hashCode += containerRelationshipTypeEntityTypeMappingCardinality.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region ICollection<ContainerRelationshipTypeEntityTypeMappingCardinality> Members

        /// <summary>
        /// Add ContainerRelationshipTypeEntityTypeMappingCardinality object in collection
        /// </summary>
        /// <param name="item">ContainerRelationshipTypeEntityTypeMappingCardinality to add in collection</param>
        public void Add(ContainerRelationshipTypeEntityTypeMappingCardinality item)
        {
            this._containerRelationshipTypeEntityTypeMappingCardinalitys.Add(item);
        }

        /// <summary>
        /// Add IContainerRelationshipTypeEntityTypeMappingCardinality in collection
        /// </summary>
        /// <param name="item">IContainerRelationshipTypeEntityTypeMappingCardinality to add in collection</param>
        public void Add(IContainerRelationshipTypeEntityTypeMappingCardinality item)
        {
            if (item != null)
            {
                this.Add((ContainerRelationshipTypeEntityTypeMappingCardinality)item);
            }
        }

        /// <summary>
        /// Add ContainerRelationshipTypeEntityTypeMappingCardinality object in collection
        /// </summary>
        /// <param name="items">ContainerRelationshipTypeEntityTypeMappingCardinality to add in collection</param>
        public void AddRange(ContainerRelationshipTypeEntityTypeMappingCardinalityCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMappingCardinality item in items)
                {
                    this._containerRelationshipTypeEntityTypeMappingCardinalitys.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes all ContainerRelationshipTypeEntityTypeMappingCardinalitys from collection
        /// </summary>
        public void Clear()
        {
            this._containerRelationshipTypeEntityTypeMappingCardinalitys.Clear();
        }

        /// <summary>
        /// Determines whether the ContainerRelationshipTypeEntityTypeMappingCardinalityCollection contains a specific ContainerRelationshipTypeEntityTypeMappingCardinality.
        /// </summary>
        /// <param name="item">The ContainerRelationshipTypeEntityTypeMappingCardinality object to locate in the ContainerRelationshipTypeEntityTypeMappingCardinalityCollection.</param>
        /// <returns>
        /// <para>true : If ContainerRelationshipTypeEntityTypeMappingCardinality found in mappingCollection</para>
        /// <para>false : If ContainerRelationshipTypeEntityTypeMappingCardinality found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ContainerRelationshipTypeEntityTypeMappingCardinality item)
        {
            return this._containerRelationshipTypeEntityTypeMappingCardinalitys.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ContainerRelationshipTypeEntityTypeMappingCardinalityCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ContainerRelationshipTypeEntityTypeMappingCardinalityCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ContainerRelationshipTypeEntityTypeMappingCardinality[] array, int arrayIndex)
        {
            this._containerRelationshipTypeEntityTypeMappingCardinalitys.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ContainerRelationshipTypeEntityTypeMappingCardinality in ContainerRelationshipTypeEntityTypeMappingCardinalityCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._containerRelationshipTypeEntityTypeMappingCardinalitys.Count;
            }
        }

        /// <summary>
        /// Check if ContainerRelationshipTypeEntityTypeMappingCardinalityCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ContainerRelationshipTypeEntityTypeMappingCardinalityCollection.
        /// </summary>
        /// <param name="item">The ContainerRelationshipTypeEntityTypeMappingCardinality object to remove from the ContainerRelationshipTypeEntityTypeMappingCardinalityCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ContainerRelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        public bool Remove(ContainerRelationshipTypeEntityTypeMappingCardinality item)
        {
            return this._containerRelationshipTypeEntityTypeMappingCardinalitys.Remove(item);
        }

        #endregion

        #region IEnumerable<ContainerRelationshipTypeEntityTypeMappingCardinality> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerRelationshipTypeEntityTypeMappingCardinalityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ContainerRelationshipTypeEntityTypeMappingCardinality> GetEnumerator()
        {
            return this._containerRelationshipTypeEntityTypeMappingCardinalitys.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerRelationshipTypeEntityTypeMappingCardinalityCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._containerRelationshipTypeEntityTypeMappingCardinalitys.GetEnumerator();
        }

        #endregion

        #region IRelationshipTypeEntityTypeMappingCollection Members

        /// <summary>
        /// Get Xml representation of ContainerRelationshipTypeEntityTypeMappingCardinalityCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerRelationshipTypeEntityTypeMappingCardinalityCollection</returns>
        public String ToXml()
        {
            String containerRelationshipTypeEntityTypeMappingCardinalitysXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in this._containerRelationshipTypeEntityTypeMappingCardinalitys)
            {
                builder.Append(containerRelationshipTypeEntityTypeMappingCardinality.ToXML());
            }

            containerRelationshipTypeEntityTypeMappingCardinalitysXml = String.Format("<ContainerRelationshipTypeEntityTypeMappingCardinalitys>{0}</ContainerRelationshipTypeEntityTypeMappingCardinalitys>", builder.ToString());

            return containerRelationshipTypeEntityTypeMappingCardinalitysXml;
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

            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = Get(referenceIds);

            if (containerRelationshipTypeEntityTypeMappingCardinalitys != null && containerRelationshipTypeEntityTypeMappingCardinalitys.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in containerRelationshipTypeEntityTypeMappingCardinalitys)
                {
                    result = result && this.Remove(containerRelationshipTypeEntityTypeMappingCardinality);
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
            Collection<IDataModelObjectCollection> containerRelationshipTypeEntityTypeMappingCardinalitiesInBatch = null;

            if (this._containerRelationshipTypeEntityTypeMappingCardinalitys != null)
            {
                containerRelationshipTypeEntityTypeMappingCardinalitiesInBatch = Utility.Split(this, batchSize);
            }

            return containerRelationshipTypeEntityTypeMappingCardinalitiesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as ContainerRelationshipTypeEntityTypeMappingCardinality);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the ContainerRelationshipTypeEntityTypeMappingCardinalityCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>ContainerRelationshipTypeEntityTypeMappingCardinalityCollection having given referenceIds</returns>
        private ContainerRelationshipTypeEntityTypeMappingCardinalityCollection Get(Collection<String> referenceIds)
        {
            ContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalitys = new ContainerRelationshipTypeEntityTypeMappingCardinalityCollection();
            Int32 counter = 0;

            if (this._containerRelationshipTypeEntityTypeMappingCardinalitys != null && this._containerRelationshipTypeEntityTypeMappingCardinalitys.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMappingCardinality containerRelationshipTypeEntityTypeMappingCardinality in this._containerRelationshipTypeEntityTypeMappingCardinalitys)
                {
                    if (referenceIds.Contains(containerRelationshipTypeEntityTypeMappingCardinality.ReferenceId))
                    {
                        containerRelationshipTypeEntityTypeMappingCardinalitys.Add(containerRelationshipTypeEntityTypeMappingCardinality);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }
            return containerRelationshipTypeEntityTypeMappingCardinalitys;
        }

        #endregion

        #endregion
    }
}