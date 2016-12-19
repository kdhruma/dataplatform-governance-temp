using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Interfaces;
    
    /// <summary>
    /// Specifies the mapping collection object for Container RelationshipType EntityType mapping
    /// </summary>
    [DataContract]
    public class ContainerRelationshipTypeEntityTypeMappingCollection : ICollection<ContainerRelationshipTypeEntityTypeMapping>, IEnumerable<ContainerRelationshipTypeEntityTypeMapping>, IContainerRelationshipTypeEntityTypeMappingCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<ContainerRelationshipTypeEntityTypeMapping> _containerRelationshipTypeEntityTypeMappings = new Collection<ContainerRelationshipTypeEntityTypeMapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMappingCollection() : base() { }

        /// <summary>
        /// Initialize ContainerRelationshipTypeEntityTypeMappingCollection from IList
        /// </summary>
        /// <param name="ContainerRelationshipTypeEntityTypeMappingList">IList of ContainerRelationshipTypeEntityTypeMappings</param>
        public ContainerRelationshipTypeEntityTypeMappingCollection(IList<ContainerRelationshipTypeEntityTypeMapping> ContainerRelationshipTypeEntityTypeMappingList)
        {
            this._containerRelationshipTypeEntityTypeMappings = new Collection<ContainerRelationshipTypeEntityTypeMapping>(ContainerRelationshipTypeEntityTypeMappingList);
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
                return MDM.Core.ObjectType.ContainerRelationshipTypeEntityTypeMapping;
            }
        }

        #endregion

        #region Method

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ContainerRelationshipTypeEntityTypeMappingCollection)
            {
                ContainerRelationshipTypeEntityTypeMappingCollection objectToBeCompared = obj as ContainerRelationshipTypeEntityTypeMappingCollection;
                Int32 ContainerRelationshipTypeEntityTypeMappingsUnion = this._containerRelationshipTypeEntityTypeMappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 ContainerRelationshipTypeEntityTypeMappingsIntersect = this._containerRelationshipTypeEntityTypeMappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (ContainerRelationshipTypeEntityTypeMappingsUnion != ContainerRelationshipTypeEntityTypeMappingsIntersect)
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
            foreach (ContainerRelationshipTypeEntityTypeMapping ContainerRelationshipTypeEntityTypeMapping in this._containerRelationshipTypeEntityTypeMappings)
            {
                hashCode += ContainerRelationshipTypeEntityTypeMapping.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Gets the ContainerRelationshipTypeEntityTypeMapping for given ContainerName and EntityTypeName.
        /// </summary>
        /// <param name="organizationName">OrganizationName to be searched in the collection</param>
        /// <param name="containerName">ContainerName to be searched in the collection</param>
        /// <param name="entityTypeName">EntityTypeName to be searched in the collection</param>
        /// <param name="relationshipTypeName">RelationshipTypeName to be searched in the collection</param>
        /// <returns>ContainerRelationshipTypeEntityTypeMapping having given OrganizationName, ContainerName, RelationshipTypeName and EntityTypeName</returns>
        public ContainerRelationshipTypeEntityTypeMapping Get(String organizationName, String containerName, String entityTypeName, String relationshipTypeName)
        {
            if (this._containerRelationshipTypeEntityTypeMappings != null && this._containerRelationshipTypeEntityTypeMappings.Count > 0)
            {
                organizationName = organizationName.ToLowerInvariant();
                containerName = containerName.ToLowerInvariant();
                entityTypeName = entityTypeName.ToLowerInvariant();
                relationshipTypeName = relationshipTypeName.ToLowerInvariant();

                foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in this._containerRelationshipTypeEntityTypeMappings)
                {
                    if (containerRelationshipTypeEntityTypeMapping.OrganizationName.ToLowerInvariant().Equals(organizationName) && 
                        containerRelationshipTypeEntityTypeMapping.ContainerName.ToLowerInvariant().Equals(containerName) && 
                        containerRelationshipTypeEntityTypeMapping.EntityTypeName.ToLowerInvariant().Equals(entityTypeName) && 
                        containerRelationshipTypeEntityTypeMapping.RelationshipTypeName.ToLowerInvariant().Equals(relationshipTypeName))
                    {
                        return containerRelationshipTypeEntityTypeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the ContainerRelationshipTypeEntityTypeMapping for given parameters.
        /// </summary>
        /// <param name="containerId">ContainerId to be searched in the collection</param>
        /// <param name="entityTypeId">EntityTypeId to be searched in the collection</param>
        /// <param name="relationshipTypeId">RelationshipTypeId to be searched in the collection</param>
        /// <returns>ContainerRelationshipTypeEntityTypeMapping having given OrganizationName, ContainerName, RelationshipTypeName and EntityTypeName</returns>
        public ContainerRelationshipTypeEntityTypeMapping Get(Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId)
        {
            if (this._containerRelationshipTypeEntityTypeMappings != null && this._containerRelationshipTypeEntityTypeMappings.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in this._containerRelationshipTypeEntityTypeMappings)
                {
                    if (containerRelationshipTypeEntityTypeMapping.ContainerId == containerId &&
                        containerRelationshipTypeEntityTypeMapping.EntityTypeId == entityTypeId &&
                        containerRelationshipTypeEntityTypeMapping.RelationshipTypeId == relationshipTypeId)
                    {
                        return containerRelationshipTypeEntityTypeMapping;
                    }
                }
            }

            return null;
        }


        #region ICollection<ContainerRelationshipTypeEntityTypeMapping> Members

        /// <summary>
        /// Add ContainerRelationshipTypeEntityTypeMapping object in collection
        /// </summary>
        /// <param name="item">ContainerRelationshipTypeEntityTypeMapping to add in collection</param>
        public void Add(ContainerRelationshipTypeEntityTypeMapping item)
        {
            this._containerRelationshipTypeEntityTypeMappings.Add(item);
        }

        /// <summary>
        /// Add ContainerRelationshipTypeEntityTypeMapping object in collection
        /// </summary>
        /// <param name="items">Indicates collection of container relationship type entity type mapping to add</param>
        public void AddRange(ContainerRelationshipTypeEntityTypeMappingCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMapping item in items)
                {
                    this._containerRelationshipTypeEntityTypeMappings.Add(item);
                }
            }
        }

        /// <summary>
        /// Add ContainerRelationshipTypeEntityTypeMapping in collection
        /// </summary>
        /// <param name="iContainerRelationshipTypeEntityTypeMapping">ContainerRelationshipTypeEntityTypeMapping to add in collection</param>
        public void Add(IContainerRelationshipTypeEntityTypeMapping iContainerRelationshipTypeEntityTypeMapping)
        {
            if (iContainerRelationshipTypeEntityTypeMapping != null)
            {
                this.Add((ContainerRelationshipTypeEntityTypeMapping)iContainerRelationshipTypeEntityTypeMapping);
            }
        }

        /// <summary>
        /// Removes all ContainerRelationshipTypeEntityTypeMappings from collection
        /// </summary>
        public void Clear()
        {
            this._containerRelationshipTypeEntityTypeMappings.Clear();
        }

        /// <summary>
        /// Determines whether the ContainerRelationshipTypeEntityTypeMappingCollection contains a specific ContainerRelationshipTypeEntityTypeMapping.
        /// </summary>
        /// <param name="item">The ContainerRelationshipTypeEntityTypeMapping object to locate in the ContainerRelationshipTypeEntityTypeMappingCollection.</param>
        /// <returns>
        /// <para>true : If ContainerRelationshipTypeEntityTypeMapping found in mappingCollection</para>
        /// <para>false : If ContainerRelationshipTypeEntityTypeMapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ContainerRelationshipTypeEntityTypeMapping item)
        {
            return this._containerRelationshipTypeEntityTypeMappings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ContainerRelationshipTypeEntityTypeMappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ContainerRelationshipTypeEntityTypeMappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ContainerRelationshipTypeEntityTypeMapping[] array, int arrayIndex)
        {
            this._containerRelationshipTypeEntityTypeMappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ContainerRelationshipTypeEntityTypeMappings in ContainerRelationshipTypeEntityTypeMappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._containerRelationshipTypeEntityTypeMappings.Count;
            }
        }

        /// <summary>
        /// Check if ContainerRelationshipTypeEntityTypeMappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ContainerRelationshipTypeEntityTypeMappingCollection.
        /// </summary>
        /// <param name="item">The ContainerRelationshipTypeEntityTypeMapping object to remove from the ContainerRelationshipTypeEntityTypeMappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ContainerRelationshipTypeEntityTypeMappingCollection</returns>
        public bool Remove(ContainerRelationshipTypeEntityTypeMapping item)
        {
            return this._containerRelationshipTypeEntityTypeMappings.Remove(item);
        }

        #endregion

        #region IEnumerable<ContainerRelationshipTypeEntityTypeMapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerRelationshipTypeEntityTypeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ContainerRelationshipTypeEntityTypeMapping> GetEnumerator()
        {
            return this._containerRelationshipTypeEntityTypeMappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerRelationshipTypeEntityTypeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._containerRelationshipTypeEntityTypeMappings.GetEnumerator();
        }

        #endregion

        #region IContainerRelationshipTypeEntityTypeMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ContainerRelationshipTypeEntityTypeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerRelationshipTypeEntityTypeMappingCollection</returns>
        public String ToXml()
        {
            String ContainerRelationshipTypeEntityTypeMappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ContainerRelationshipTypeEntityTypeMapping ContainerRelationshipTypeEntityTypeMapping in this._containerRelationshipTypeEntityTypeMappings)
            {
                builder.Append(ContainerRelationshipTypeEntityTypeMapping.ToXML());
            }

            ContainerRelationshipTypeEntityTypeMappingsXml = String.Format("<ContainerEntityTypes>{0}</ContainerEntityTypes>", builder.ToString());

            return ContainerRelationshipTypeEntityTypeMappingsXml;
        }

        #endregion ToXml methods

        #endregion IContainerRelationshipTypeEntityTypeMappingCollection Members

        #region IDataModelObjectCollection
        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = Get(referenceIds);

            if (containerRelationshipTypeEntityTypeMappings != null && containerRelationshipTypeEntityTypeMappings.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in containerRelationshipTypeEntityTypeMappings)
                {
                    result = result && this.Remove(containerRelationshipTypeEntityTypeMapping);
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
            Collection<IDataModelObjectCollection> containerRelationshipTypeEntityTypeMappingsInBatch = null;

            if (this._containerRelationshipTypeEntityTypeMappings != null)
            {
                containerRelationshipTypeEntityTypeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return containerRelationshipTypeEntityTypeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as ContainerRelationshipTypeEntityTypeMapping);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the ContainerRelationshipTypeEntityTypeMappingCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>ContainerRelationshipTypeEntityTypeMappingCollection having given referenceIds</returns>
        private ContainerRelationshipTypeEntityTypeMappingCollection Get(Collection<String> referenceIds)
        {
            ContainerRelationshipTypeEntityTypeMappingCollection containerRelationshipTypeEntityTypeMappings = new ContainerRelationshipTypeEntityTypeMappingCollection();
            Int32 counter = 0;

            if (this._containerRelationshipTypeEntityTypeMappings != null && this._containerRelationshipTypeEntityTypeMappings.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (ContainerRelationshipTypeEntityTypeMapping containerRelationshipTypeEntityTypeMapping in this._containerRelationshipTypeEntityTypeMappings)
                {
                    if (referenceIds.Contains(containerRelationshipTypeEntityTypeMapping.ReferenceId))
                    {
                        containerRelationshipTypeEntityTypeMappings.Add(containerRelationshipTypeEntityTypeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }
            return containerRelationshipTypeEntityTypeMappings;
        }

        #endregion

        #endregion
    }
}
