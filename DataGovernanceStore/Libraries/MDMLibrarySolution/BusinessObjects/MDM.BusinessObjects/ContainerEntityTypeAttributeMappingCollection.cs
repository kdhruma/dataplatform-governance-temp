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
    /// Specifies the Container Entity Type Attribute Mapping Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class ContainerEntityTypeAttributeMappingCollection : ICollection<ContainerEntityTypeAttributeMapping>, IEnumerable<ContainerEntityTypeAttributeMapping>, IContainerEntityTypeAttributeMappingCollection, IDataModelObjectCollection
    {
        #region Fields

        [DataMember]
        private Collection<ContainerEntityTypeAttributeMapping> _containerEntityTypeAttributeMappings = new Collection<ContainerEntityTypeAttributeMapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public ContainerEntityTypeAttributeMappingCollection() : base() { }

        /// <summary>
        /// Initialize ContainerEntityTypeAttributeMappingCollection from IList
        /// </summary>
        /// <param name="ContainerEntityTypeAttributeMappingList">IList of ContainerEntityTypeAttributeMappings</param>
        public ContainerEntityTypeAttributeMappingCollection(IList<ContainerEntityTypeAttributeMapping> ContainerEntityTypeAttributeMappingList)
        {
            this._containerEntityTypeAttributeMappings = new Collection<ContainerEntityTypeAttributeMapping>(ContainerEntityTypeAttributeMappingList);
        }

        /// <summary>
        /// Initializes a new instance of the ContainerEntityTypeAttributeMappingCollection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public ContainerEntityTypeAttributeMappingCollection(String valuesAsXml)
        {
            LoadContainerEntityTypeAttributeMappingCollection(valuesAsXml);
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
                return MDM.Core.ObjectType.ContainerEntityTypeAttributeMapping;
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
        public override Boolean Equals(object obj)
        {
            if (obj is ContainerEntityTypeAttributeMappingCollection)
            {
                ContainerEntityTypeAttributeMappingCollection objectToBeCompared = obj as ContainerEntityTypeAttributeMappingCollection;
                Int32 containerEntityTypeAttributeMappingsUnion = this._containerEntityTypeAttributeMappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 containerEntityTypeAttributeMappingsIntersect = this._containerEntityTypeAttributeMappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (containerEntityTypeAttributeMappingsUnion != containerEntityTypeAttributeMappingsIntersect)
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
            foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in this._containerEntityTypeAttributeMappings)
            {
                hashCode += containerEntityTypeAttributeMapping.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMapping GetByAttributeId(Int32 attributeId)
        {
            return GetByAttributeIds(new Collection<Int32>() { attributeId }).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMappingCollection GetByAttributeIds(Collection<Int32> attributeIds)
        {
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = null;

            if (this._containerEntityTypeAttributeMappings != null && this._containerEntityTypeAttributeMappings.Count > 0
                && attributeIds != null && attributeIds.Count > 0)
            {
                containerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();

                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in this._containerEntityTypeAttributeMappings)
                {
                    if (attributeIds.Contains(containerEntityTypeAttributeMapping.AttributeId))
                    {
                        containerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMapping);
                    }

                    if (containerEntityTypeAttributeMappings.Count.Equals(attributeIds.Count)) break;
                }
            }

            return containerEntityTypeAttributeMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeGroupId"></param>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMappingCollection GetByAttributeGroupId(Int32 attributeGroupId)
        {
            return GetByAttributeGroupIds(new Collection<Int32>() { attributeGroupId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeGroupIds"></param>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMappingCollection GetByAttributeGroupIds(Collection<Int32> attributeGroupIds)
        {
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = null;

            if (this._containerEntityTypeAttributeMappings != null && this._containerEntityTypeAttributeMappings.Count > 0
                && attributeGroupIds != null && attributeGroupIds.Count > 0)
            {
                containerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();

                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in this._containerEntityTypeAttributeMappings)
                {
                    if (attributeGroupIds.Contains(containerEntityTypeAttributeMapping.AttributeParentId))
                    {
                        containerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMapping);
                    }
                }
            }

            return containerEntityTypeAttributeMappings;
        }

        /// <summary>
        /// This method returns all mappings if attribute id and attribute group id are not available.
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeGroupId"></param>
        /// <returns></returns>
        public ContainerEntityTypeAttributeMapping GetByAttributeIdAndGroupId(Int32 attributeId, Int32 attributeGroupId)
        {
            if (this._containerEntityTypeAttributeMappings != null && this._containerEntityTypeAttributeMappings.Count > 0)
            {
                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in this._containerEntityTypeAttributeMappings)
                {
                    if (containerEntityTypeAttributeMapping.AttributeId.Equals(attributeId)
                        && containerEntityTypeAttributeMapping.AttributeParentId.Equals(attributeGroupId))
                    {
                        return containerEntityTypeAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the ContainerEntityTypeAttributeMapping for given ContainerName, EntityTypeName, attributeName and attributeParentName.
        /// </summary>
        /// /// <param name="organizationName">OrganizationName to be searched in the collection</param>
        /// <param name="containerName">ContainerName to be searched in the collection</param>
        /// <param name="entityTypeName">EntityTypeName to be searched in the collection</param>
        /// <param name="attributeName">AttributeName to be searched in the collection</param>
        /// <param name="attributeParentName">Attribute Parent Name to be searched in the collection</param>
        /// <returns>ContainerEntityTypeAttributeMapping for given ContainerName, EntityTypeName, attributeName and attributeParentName</returns>
        public ContainerEntityTypeAttributeMapping Get(String organizationName, String containerName, String entityTypeName, String attributeName, String attributeParentName)
        {
            if (this._containerEntityTypeAttributeMappings != null && this._containerEntityTypeAttributeMappings.Count > 0)
            {
                organizationName = organizationName.ToLowerInvariant();
                containerName = containerName.ToLowerInvariant();
                entityTypeName = entityTypeName.ToLowerInvariant();
                attributeName = attributeName.ToLowerInvariant();
                attributeParentName = attributeParentName.ToLowerInvariant();

                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in this._containerEntityTypeAttributeMappings)
                {
                    if (containerEntityTypeAttributeMapping.OrganizationName.ToLowerInvariant().Equals(organizationName) &&
                        containerEntityTypeAttributeMapping.ContainerName.ToLowerInvariant().Equals(containerName) &&
                        containerEntityTypeAttributeMapping.EntityTypeName.ToLowerInvariant().Equals(entityTypeName) &&
                        containerEntityTypeAttributeMapping.AttributeName.ToLowerInvariant().Equals(attributeName) &&
                        containerEntityTypeAttributeMapping.AttributeParentName.ToLowerInvariant().Equals(attributeParentName))
                    {
                        return containerEntityTypeAttributeMapping;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Compare ContainerEntityTypeAttributeMappingCollection. 
        /// </summary>
        /// <param name="subsetContainerEntityTypeAttributeMappingCollection">Expected ContainerEntityTypeAttributeMappingCollection.</param>
        /// <param name="compareIds">Compare Ids or not.</param>
        /// <returns>If actual ContainerEntityTypeAttributeMappingCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(ContainerEntityTypeAttributeMappingCollection subsetContainerEntityTypeAttributeMappingCollection, Boolean compareIds = false)
        {
            if (subsetContainerEntityTypeAttributeMappingCollection != null && subsetContainerEntityTypeAttributeMappingCollection.Count > 0)
            {
                foreach (ContainerEntityTypeAttributeMapping subsetContainerEntityTypeAttributeMapping in subsetContainerEntityTypeAttributeMappingCollection)
                {
                    ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping = this.Get(subsetContainerEntityTypeAttributeMapping.OrganizationName, subsetContainerEntityTypeAttributeMapping.ContainerName, subsetContainerEntityTypeAttributeMapping.EntityTypeName, subsetContainerEntityTypeAttributeMapping.AttributeName, subsetContainerEntityTypeAttributeMapping.AttributeParentName);

                    if (containerEntityTypeAttributeMapping == null)
                    {
                        return false;
                    }
                    else if (!containerEntityTypeAttributeMapping.IsSuperSetOf(subsetContainerEntityTypeAttributeMapping, compareIds))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #region ICollection<ContainerEntityTypeAttributeMapping> Members

        /// <summary>
        /// Add ContainerEntityTypeAttributeMapping object in collection
        /// </summary>
        /// <param name="item">ContainerEntityTypeAttributeMapping to add in collection</param>
        public void Add(ContainerEntityTypeAttributeMapping item)
        {
            this._containerEntityTypeAttributeMappings.Add(item);
        }

        /// <summary>
        /// Add ContainerEntityTypeAttributeMapping object in collection
        /// </summary>
        /// <param name="items">ContainerEntityTypeAttributeMapping to add in collection</param>
        public void AddRange(ContainerEntityTypeAttributeMappingCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (ContainerEntityTypeAttributeMapping item in items)
                {
                    this._containerEntityTypeAttributeMappings.Add(item);
                }
            }
        }

        /// <summary>
        /// Add ContainerEntityTypeAttributeMapping in collection
        /// </summary>
        /// <param name="iContainerEntityTypeAttributeMapping">ContainerEntityTypeAttributeMapping to add in collection</param>
        public void Add(IContainerEntityTypeAttributeMapping iContainerEntityTypeAttributeMapping)
        {
            if (iContainerEntityTypeAttributeMapping != null)
            {
                this.Add((ContainerEntityTypeAttributeMapping)iContainerEntityTypeAttributeMapping);
            }
        }

        /// <summary>
        /// Removes all ContainerEntityTypeAttributeMappings from collection
        /// </summary>
        public void Clear()
        {
            this._containerEntityTypeAttributeMappings.Clear();
        }

        /// <summary>
        /// Determines whether the ContainerEntityTypeAttributeMappingCollection contains a specific ContainerEntityTypeAttributeMapping.
        /// </summary>
        /// <param name="item">The ContainerEntityTypeAttributeMapping object to locate in the ContainerEntityTypeAttributeMappingCollection.</param>
        /// <returns>
        /// <para>true : If ContainerEntityTypeAttributeMapping found in mappingCollection</para>
        /// <para>false : If ContainerEntityTypeAttributeMapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ContainerEntityTypeAttributeMapping item)
        {
            return this._containerEntityTypeAttributeMappings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ContainerEntityTypeAttributeMappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ContainerEntityTypeAttributeMappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ContainerEntityTypeAttributeMapping[] array, int arrayIndex)
        {
            this._containerEntityTypeAttributeMappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ContainerEntityTypeAttributeMappings in ContainerEntityTypeAttributeMappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._containerEntityTypeAttributeMappings.Count;
            }
        }

        /// <summary>
        /// Check if ContainerEntityTypeAttributeMappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ContainerEntityTypeAttributeMappingCollection.
        /// </summary>
        /// <param name="item">The ContainerEntityTypeAttributeMapping object to remove from the ContainerEntityTypeAttributeMappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ContainerEntityTypeAttributeMappingCollection</returns>
        public bool Remove(ContainerEntityTypeAttributeMapping item)
        {
            return this._containerEntityTypeAttributeMappings.Remove(item);
        }

        #endregion

        #region IEnumerable<ContainerEntityTypeAttributeMapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerEntityTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ContainerEntityTypeAttributeMapping> GetEnumerator()
        {
            return this._containerEntityTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ContainerEntityTypeAttributeMappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._containerEntityTypeAttributeMappings.GetEnumerator();
        }

        #endregion

        #region IContainerEntityTypeAttributeMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ContainerEntityTypeAttributeMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the ContainerEntityTypeAttributeMappingCollection</returns>
        public String ToXml()
        {
            String containerEntityTypeAttributeMappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in this._containerEntityTypeAttributeMappings)
            {
                builder.Append(containerEntityTypeAttributeMapping.ToXML());
            }

            containerEntityTypeAttributeMappingsXml = String.Format("<ContainerEntityTypeAttributes>{0}</ContainerEntityTypeAttributes>", builder.ToString());

            return containerEntityTypeAttributeMappingsXml;
        }

        #endregion ToXml methods

        #endregion IContainerEntityTypeAttributeMappingCollection Memebers

        #region IDataModelObjectCollection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceIds"></param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = Get(referenceIds);

            if (containerEntityTypeAttributeMappings != null && containerEntityTypeAttributeMappings.Count > 0)
            {
                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in containerEntityTypeAttributeMappings)
                {
                    result = result && this.Remove(containerEntityTypeAttributeMapping);
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
            Collection<IDataModelObjectCollection> containerEntityTypeMappingsInBatch = new Collection<IDataModelObjectCollection>();

            if (this._containerEntityTypeAttributeMappings != null)
            {
                containerEntityTypeMappingsInBatch = Utility.Split(this, batchSize);
            }

            return containerEntityTypeMappingsInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as ContainerEntityTypeAttributeMapping);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the ContainerEntityTypeAttributeMappingCollection for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>ContainerEntityTypeAttributeMappingCollection having given referenceIds</returns>
        private ContainerEntityTypeAttributeMappingCollection Get(Collection<String> referenceIds)
        {
            ContainerEntityTypeAttributeMappingCollection containerEntityTypeAttributeMappings = new ContainerEntityTypeAttributeMappingCollection();
            Int32 counter = 0;

            if (this._containerEntityTypeAttributeMappings != null && this._containerEntityTypeAttributeMappings.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping in this._containerEntityTypeAttributeMappings)
                {
                    if (referenceIds.Contains(containerEntityTypeAttributeMapping.ReferenceId))
                    {
                        containerEntityTypeAttributeMappings.Add(containerEntityTypeAttributeMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return containerEntityTypeAttributeMappings;
        }

        /// <summary>
        /// Load current ContainerEntityTypeAttributeMappingCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current ContainerEntityTypeAttributeMappingCollection
        /// </param>
        private void LoadContainerEntityTypeAttributeMappingCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
                <ContainerEntityTypeAttributes>
	                <ContainerEntityTypeAttribute Id="-1" ContainerId="3" EntityTypeId="29" AttributeId = "4079" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" Action="Create" />
	                <ContainerEntityTypeAttribute Id="-1" ContainerId="3" EntityTypeId="29" AttributeId = "4080" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" Action="Create" />
                </ContainerEntityTypeAttributes>
            */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerEntityTypeAttribute")
                    {
                        String containerEntityTypeAttributeMappingXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(containerEntityTypeAttributeMappingXml))
                        {
                            ContainerEntityTypeAttributeMapping containerEntityTypeAttributeMapping = new ContainerEntityTypeAttributeMapping(containerEntityTypeAttributeMappingXml);
                            if (containerEntityTypeAttributeMapping != null)
                            {
                                this.Add(containerEntityTypeAttributeMapping);
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