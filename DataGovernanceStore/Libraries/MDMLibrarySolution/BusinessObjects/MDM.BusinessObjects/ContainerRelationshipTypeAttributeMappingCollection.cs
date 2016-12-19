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
    /// Specifies the Container Relationship Entity Type Attribute Mapping Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class ContainerRelationshipTypeAttributeMappingCollection : ICollection<ContainerRelationshipTypeAttributeMapping>, IEnumerable<ContainerRelationshipTypeAttributeMapping>, IContainerRelationshipTypeAttributeMappingCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<ContainerRelationshipTypeAttributeMapping> _containerRelationshipTypeAttributeMappings = new Collection<ContainerRelationshipTypeAttributeMapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public ContainerRelationshipTypeAttributeMappingCollection() : base() { }

        /// <summary>
        /// Initialize ContainerRelationshipTypeAttributeMappingCollection from IList
        /// </summary>
        /// <param name="ContainerRelationshipTypeAttributeMappingList">IList of ContainerRelationshipTypeAttributeMappings</param>
        public ContainerRelationshipTypeAttributeMappingCollection(IList<ContainerRelationshipTypeAttributeMapping> ContainerRelationshipTypeAttributeMappingList)
        {
            this._containerRelationshipTypeAttributeMappings = new Collection<ContainerRelationshipTypeAttributeMapping>(ContainerRelationshipTypeAttributeMappingList);
        }

        /// <summary>
        /// Initializes a new instance of the ContainerRelationshipTypeAttributeMappingCollection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public ContainerRelationshipTypeAttributeMappingCollection(String valuesAsXml)
        {
            LoadContainerRelationshipTypeAttributeMappingCollection(valuesAsXml);
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
                return MDM.Core.ObjectType.ContainerRelationshipTypeAttributeMapping;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ContainerRelationshipTypeAttributeMappingCollection)
            {
                ContainerRelationshipTypeAttributeMappingCollection objectToBeCompared = obj as ContainerRelationshipTypeAttributeMappingCollection;
                Int32 containerRelationshipTypeAttributeMappingsUnion = this._containerRelationshipTypeAttributeMappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 containerRelationshipTypeAttributeMappingsIntersect = this._containerRelationshipTypeAttributeMappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (containerRelationshipTypeAttributeMappingsUnion != containerRelationshipTypeAttributeMappingsIntersect)
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
            foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in this._containerRelationshipTypeAttributeMappings)
            {
                hashCode += containerRelationshipTypeAttributeMapping.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeGroupId"></param>
        /// <returns></returns>
        public ContainerRelationshipTypeAttributeMappingCollection GetByAttributeGroupId(Int32 attributeGroupId)
        {
            return GetByAttributeGroupIds(new Collection<Int32>() { attributeGroupId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeGroupIds"></param>
        /// <returns></returns>
        public ContainerRelationshipTypeAttributeMappingCollection GetByAttributeGroupIds(Collection<Int32> attributeGroupIds)
        {
            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = null;

            if (this._containerRelationshipTypeAttributeMappings != null && this._containerRelationshipTypeAttributeMappings.Count > 0
                && attributeGroupIds != null && attributeGroupIds.Count > 0)
            {
                containerRelationshipTypeAttributeMappings = new ContainerRelationshipTypeAttributeMappingCollection();

                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in this._containerRelationshipTypeAttributeMappings)
                {
                    if (attributeGroupIds.Contains(containerRelationshipTypeAttributeMapping.AttributeParentId))
                    {
                        containerRelationshipTypeAttributeMappings.Add(containerRelationshipTypeAttributeMapping);
                    }
                }
            }

            return containerRelationshipTypeAttributeMappings;
        }

        /// <summary>
        /// Gets the ContainerRelationshipTypeAttributeMapping for given ContainerName and EntityTypeName.
        /// </summary>
        /// <param name="organizationName">EntityTypeName to be searched in the collection</param>
        /// <param name="containerName">EntityTypeName to be searched in the collection</param>
        /// <param name="relationshipTypeName">AttributeName to be searched in the collection</param>
        /// <param name="attributeName">AttributeName to be searched in the collection</param>
        /// <param name="attributeParentName">AttributeName to be searched in the collection</param>
        /// <returns>ContainerRelationshipTypeAttributeMapping having given ContainerName and EntityTypeName</returns>
        public ContainerRelationshipTypeAttributeMapping Get(String organizationName, String containerName, String relationshipTypeName, String attributeName, String attributeParentName)
        {
            if (this._containerRelationshipTypeAttributeMappings != null && this._containerRelationshipTypeAttributeMappings.Count > 0)
            {
                organizationName = organizationName.ToLowerInvariant();
                containerName = containerName.ToLowerInvariant();
                relationshipTypeName = relationshipTypeName.ToLowerInvariant();
                attributeName = attributeName.ToLowerInvariant();
                attributeParentName = attributeParentName.ToLowerInvariant();

                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in this._containerRelationshipTypeAttributeMappings)
                {
                    if (containerRelationshipTypeAttributeMapping.OrganizationName.ToLowerInvariant().Equals(organizationName) &&
                        containerRelationshipTypeAttributeMapping.ContainerName.ToLowerInvariant().Equals(containerName) &&
                        containerRelationshipTypeAttributeMapping.RelationshipTypeName.ToLowerInvariant().Equals(relationshipTypeName) &&
                        containerRelationshipTypeAttributeMapping.AttributeName.ToLowerInvariant().Equals(attributeName) &&
                        containerRelationshipTypeAttributeMapping.AttributeParentName.ToLowerInvariant().Equals(attributeParentName))
                    {
                        return containerRelationshipTypeAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Compare ContainerRelationshipTypeAttributeMappingCollection. 
        /// </summary>
        /// <param name="subsetContainerRelationshipTypeAttributeMappingCollection">Expected ContainerRelationshipTypeAttributeMappingCollection.</param>
        /// <param name="compareIds">Compare Ids or not.</param>
        /// <returns>If actual ContainerRelationshipTypeAttributeMappingCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(ContainerRelationshipTypeAttributeMappingCollection subsetContainerRelationshipTypeAttributeMappingCollection, Boolean compareIds = false)
        {
            if (subsetContainerRelationshipTypeAttributeMappingCollection != null && subsetContainerRelationshipTypeAttributeMappingCollection.Count > 0)
            {
                foreach (ContainerRelationshipTypeAttributeMapping subsetContainerRelationshipTypeAttributeMapping in subsetContainerRelationshipTypeAttributeMappingCollection)
                {
                    ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = this.Get(subsetContainerRelationshipTypeAttributeMapping.OrganizationName, subsetContainerRelationshipTypeAttributeMapping.ContainerName, subsetContainerRelationshipTypeAttributeMapping.RelationshipTypeName, subsetContainerRelationshipTypeAttributeMapping.AttributeName, subsetContainerRelationshipTypeAttributeMapping.AttributeParentName);

                    if (containerRelationshipTypeAttributeMapping == null)
                    {
                        return false;
                    }
                    else if (!containerRelationshipTypeAttributeMapping.IsSuperSetOf(subsetContainerRelationshipTypeAttributeMapping, compareIds))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        #region ICollection<ContainerRelationshipTypeAttributeMapping> Members

        /// <summary>
        /// Add ContainerRelationshipTypeAttributeMapping object in collection
        /// </summary>
        /// <param name="item">ContainerRelationshipTypeAttributeMapping to add in collection</param>
        public void Add(ContainerRelationshipTypeAttributeMapping item)
        {
            this._containerRelationshipTypeAttributeMappings.Add(item);
        }

        /// <summary>
        /// Add ContainerRelationshipTypeAttributeMapping in collection
        /// </summary>
        /// <param name="iContainerRelationshipTypeAttributeMapping">ContainerRelationshipTypeAttributeMapping to add in collection</param>
        public void Add(IContainerRelationshipTypeAttributeMapping iContainerRelationshipTypeAttributeMapping)
        {
            if (iContainerRelationshipTypeAttributeMapping != null)
            {
                this.Add((ContainerRelationshipTypeAttributeMapping)iContainerRelationshipTypeAttributeMapping);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public IContainerRelationshipTypeAttributeMappingCollection AddRange(IContainerRelationshipTypeAttributeMappingCollection collection)
        {
            foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in collection)
            {
                if (!this._containerRelationshipTypeAttributeMappings.Contains(containerRelationshipTypeAttributeMapping))
                {
                    this._containerRelationshipTypeAttributeMappings.Add(containerRelationshipTypeAttributeMapping);
                }
            }
            return this;
        }

        /// <summary>
        /// Removes all ContainerRelationshipTypeAttributeMappings from collection
        /// </summary>
        public void Clear()
        {
            this._containerRelationshipTypeAttributeMappings.Clear();
        }

        /// <summary>
        /// Determines whether the ContainerRelationshipTypeAttributeMappingCollection contains a specific ContainerRelationshipTypeAttributeMapping.
        /// </summary>
        /// <param name="item">The ContainerRelationshipTypeAttributeMapping object to locate in the ContainerRelationshipTypeAttributeMappingCollection.</param>
        /// <returns>
        /// <para>true : If ContainerRelationshipTypeAttributeMapping found in mappingCollection</para>
        /// <para>false : If ContainerRelationshipTypeAttributeMapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ContainerRelationshipTypeAttributeMapping item)
        {
            return this._containerRelationshipTypeAttributeMappings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ContainerRelationshipTypeAttributeMappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ContainerRelationshipTypeAttributeMappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ContainerRelationshipTypeAttributeMapping[] array, int arrayIndex)
        {
            this._containerRelationshipTypeAttributeMappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ContainerRelationshipTypeAttributeMappings in ContainerRelationshipTypeAttributeMappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._containerRelationshipTypeAttributeMappings.Count;
            }
        }

        /// <summary>
        /// Check if ContainerRelationshipTypeAttributeMappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ContainerRelationshipTypeAttributeMappingCollection.
        /// </summary>
        /// <param name="item">The ContainerRelationshipTypeAttributeMapping object to remove from the ContainerRelationshipTypeAttributeMappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ContainerRelationshipTypeAttributeMappingCollection</returns>
        public bool Remove(ContainerRelationshipTypeAttributeMapping item)
        {
            return this._containerRelationshipTypeAttributeMappings.Remove(item);
        }

        #endregion

        #region IEnumerable<ContainerRelationshipTypeAttributeMapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerRelationshipTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ContainerRelationshipTypeAttributeMapping> GetEnumerator()
        {
            return this._containerRelationshipTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerRelationshipTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._containerRelationshipTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IContainerRelationshipTypeAttributeMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ContainerRelationshipTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerRelationshipTypeAttributeMappingCollection</returns>
        public String ToXml()
        {
            String containerRelationshipTypeAttributeMappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in this._containerRelationshipTypeAttributeMappings)
            {
                builder.Append(containerRelationshipTypeAttributeMapping.ToXML());
            }

            containerRelationshipTypeAttributeMappingsXml = String.Format("<ContainerRelationshipTypeAttributes>{0}</ContainerRelationshipTypeAttributes>", builder.ToString());

            return containerRelationshipTypeAttributeMappingsXml;
        }

        #endregion ToXml methods

        #endregion IContainerRelationshipTypeAttributeMappingCollection Memebers

        #region IDataModelObjectCollection

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = Get(referenceIds);

            if (containerRelationshipTypeAttributeMappings != null && containerRelationshipTypeAttributeMappings.Count > 0)
            {
                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in containerRelationshipTypeAttributeMappings)
                {
                    result = result && this.Remove(containerRelationshipTypeAttributeMapping);
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
            Collection<IDataModelObjectCollection> containerRelationshipTypeAttributeMappingsInBatch = null;

            if (this._containerRelationshipTypeAttributeMappings != null)
            {
                containerRelationshipTypeAttributeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return containerRelationshipTypeAttributeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as ContainerRelationshipTypeAttributeMapping);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the ContainerRelationshipTypeAttributeMappingCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>ContainerRelationshipTypeAttributeMappingCollection having given referenceIds</returns>
        private ContainerRelationshipTypeAttributeMappingCollection Get(Collection<String> referenceIds)
        {
            ContainerRelationshipTypeAttributeMappingCollection containerRelationshipTypeAttributeMappings = new ContainerRelationshipTypeAttributeMappingCollection();
            Int32 counter = 0;

            if (this._containerRelationshipTypeAttributeMappings != null && this._containerRelationshipTypeAttributeMappings.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping in this._containerRelationshipTypeAttributeMappings)
                {
                    if (referenceIds.Contains(containerRelationshipTypeAttributeMapping.ReferenceId))
                    {
                        containerRelationshipTypeAttributeMappings.Add(containerRelationshipTypeAttributeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }
            return containerRelationshipTypeAttributeMappings;
        }

        /// <summary>
        /// Load current ContainerRelationshipTypeAttributeMappingCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current ContainerRelationshipTypeAttributeMappingCollection
        /// </param>
        private void LoadContainerRelationshipTypeAttributeMappingCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
                <ContainerRelationshipTypeAttributes>
	                <ContainerRelationshipTypeAttribute Id="-1" ContainerId = "4" RelationshipTypeId = "6" AttributeId = "4433" ShowAtCreation = "False" Required = "N" ReadOnly = "False" SortOrder = "0" ShowInline = "False" Action="Create" />
	                <ContainerRelationshipTypeAttribute Id="-1" ContainerId = "4" RelationshipTypeId = "6" AttributeId = "4434" ShowAtCreation = "False" Required = "N" ReadOnly = "False" SortOrder = "0" ShowInline = "False" Action="Create" />
                </ContainerRelationshipTypeAttributes>
            */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerRelationshipTypeAttribute")
                    {
                        String containerRelationshipTypeAttributeMappingXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(containerRelationshipTypeAttributeMappingXml))
                        {
                            ContainerRelationshipTypeAttributeMapping containerRelationshipTypeAttributeMapping = new ContainerRelationshipTypeAttributeMapping(containerRelationshipTypeAttributeMappingXml);
                            if (containerRelationshipTypeAttributeMapping != null)
                            {
                                this.Add(containerRelationshipTypeAttributeMapping);
                            }
                        }
                    }
                    else
                    {
                        reader.Read();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion
        
        #endregion
    }
}